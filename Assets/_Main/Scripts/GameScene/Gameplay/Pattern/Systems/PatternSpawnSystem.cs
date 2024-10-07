using System;
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.GameScene.MonoInstallers;
using _Main.Scripts.Pattern;
using _Main.Scripts.Spawn;
using App.Scripts.Modules.EcsWorld.Common.Extensions;
using App.Scripts.Modules.Pool.Interfaces.Pool;
using App.Scripts.Modules.Utils.RandomService;
using mattatz.Triangulation2DSystem;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts.Gameplay.GameBoard
{
    public class PatternSpawnSystem : ISystem
    {
        private readonly IPool<ShapeView> _pool;
        private readonly PatternDrawingConfig _patternDrawingConfig;
        private readonly RenderConfig _renderConfig;
        private readonly IRandomService _randomService;
        private readonly GameBoardContent _gameBoardContent;

        private Filter _createPatternShapesFilter;

        public World World { get; set; }

        public PatternSpawnSystem(IPool<ShapeView> pool, PatternDrawingConfig patternDrawingConfig, 
            RenderConfig renderConfig, IRandomService randomService, GameBoardContent gameBoardContent)
        {
            _pool = pool;
            _patternDrawingConfig = patternDrawingConfig;
            _renderConfig = renderConfig;
            _randomService = randomService;
            _gameBoardContent = gameBoardContent;
        }

        public void OnAwake()
        {
            _createPatternShapesFilter = World.Filter
                .With<CreatePatternSignal>()
                .With<ShapeSpawnSignal>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_createPatternShapesFilter.TryGetFirstEntity(out var patternEntity))
            {
                return;
            }
            
            Entity patterEntity = TryCreatePattern(patternEntity);
            if (patterEntity != null)
            {
                CreateShapesByPattern(patterEntity.GetComponent<ShapeComponent>().Triangles);
            }
        }

        private Entity TryCreatePattern(Entity patternEntity)
        {
            ref var patternSignal = ref patternEntity.GetComponent<CreatePatternSignal>();
            ref var shapeSignal = ref patternEntity.GetComponent<ShapeSpawnSignal>();

            var points = patternSignal.Points;
            if (points.Count < 2)
            {
                return null;
            }

            points = Utils2D.Constrain(points, _patternDrawingConfig.Threshold);
            Polygon2D polygon = Polygon2D.Contour(points.ToArray());
            Vertex2D[] vertices = polygon.Vertices;

            if (vertices.Length < 3)
            {
                return null;
            }

            Triangulation2D triangulation;
            try
            {
                triangulation = new Triangulation2D(polygon, _patternDrawingConfig.Angle);
            }
            catch (Exception)
            {
                return null;
            }

            Mesh mesh = triangulation.Build();
            
            ShapeView patternView = _pool.Get();
            patternView.Init(patternEntity, _renderConfig.PatternMaterial);
            patternView.SetupTransformProperties(shapeSignal.Parent, shapeSignal.Position, shapeSignal.Size);
            patternView.MeshFilter.sharedMesh = mesh;
            
            List<Vector3> externalPoints = ShapeUtils.FindExternalPoints(triangulation.Triangles, patternView.transform.position);
            
            ShapeComponent patternShapeComponent = new ShapeComponent
            {
                Points = mesh.vertices,
                Triangles = triangulation.Triangles,
                ShapeView = patternView,
                ExternalPoints = externalPoints
            };
            
            patternEntity.SetComponent(patternShapeComponent);
            patternEntity.RemoveComponent<CreatePatternSignal>();
            patternEntity.RemoveComponent<ShapeSpawnSignal>();
            patternEntity.AddComponent<PatternMarker>();
            // patternEntity.AddComponent<ShapeRenderMarker>();
            
            return patternEntity;
        }

        private void CreateShapesByPattern(Triangle2D[] triangles)
        {
            List<Triangle2D> usedTriangles = new();
            List<Triangle2D> activeTriangles = new();
            List<Triangle2D> allTriangles = new(triangles);
            List<List<Triangle2D>> shapes = new();
			
            int shapesCount = _randomService.Range(4, 11);
            shapesCount = Math.Min(shapesCount, allTriangles.Count);
            int allTrianglesCount = allTriangles.Count;

            while (allTriangles.Count != 0)
            {
                int firstTriangleIndex = _randomService.Range(0, allTriangles.Count);
                Triangle2D firstTriangle = allTriangles[firstTriangleIndex];
                activeTriangles.Add(firstTriangle);
                int maxCount = _randomService.Range(1, allTrianglesCount / shapesCount);
                maxCount = Math.Min(maxCount, allTriangles.Count);
				
                CreateShape(allTriangles, usedTriangles, activeTriangles, maxCount);
				
                shapes.Add(new List<Triangle2D>(usedTriangles));
                allTriangles.RemoveAll(el => usedTriangles.Contains(el));
                usedTriangles.Clear();
            }

            foreach (var shapeTriangles in shapes)
            {
                var shapeEntity = World.CreateEntity();
                
                shapeEntity.SetComponent(new ShapeSpawnSignal
                {
                    Parent = _gameBoardContent.ShapesContent,
                    Size = Vector3.one,
                    Triangles = shapeTriangles.ToArray()
                });
            }
        }
        
        private void CreateShape(List<Triangle2D> allTriangles, List<Triangle2D> usedTriangles, List<Triangle2D> activeTriangles, int leftCount)
        {
            List<Triangle2D> newActiveTriangles = new();
			
            usedTriangles.AddRange(activeTriangles);
			
            if (leftCount == 0 || activeTriangles.Count == 0)
            {
                activeTriangles.Clear();
                return;
            }
			
            activeTriangles.ShuffleRandom(_randomService);

            foreach (Triangle2D activeTriangle in activeTriangles)
            {
                if (leftCount <= 0)
                {
                    break;
                }
				
                List<Triangle2D> adjacentTriangles = FindAdjacentTriangles(allTriangles, activeTriangle);

                adjacentTriangles = adjacentTriangles.Where(el => !usedTriangles.Contains(el)).ToList();
                int adjacentCount = Math.Min(leftCount, _randomService.Range(1, 3));
				
                if (adjacentTriangles.Count == 0)
                {
                    continue;
                }

                adjacentCount = Math.Min(adjacentCount, adjacentTriangles.Count);

                adjacentTriangles.ShuffleRandom(_randomService);
				
                newActiveTriangles.AddRange(adjacentTriangles.GetRange(0, adjacentCount));

                leftCount -= adjacentCount;
            }
			
            activeTriangles.Clear();
            activeTriangles.AddRange(newActiveTriangles);

            CreateShape(allTriangles, usedTriangles, activeTriangles, leftCount);
        }
        
        private List<Triangle2D> FindAdjacentTriangles(List<Triangle2D> allTriangles, Triangle2D selectedTriangle)
        {
            List<Triangle2D> adjacentTriangles = new();
            foreach (Triangle2D triangle in allTriangles)
            {
                int commonCount = selectedTriangle.ContactPointsCount(triangle);

                if (commonCount == 2)
                {
                    adjacentTriangles.Add(triangle);
                }
            }

            return adjacentTriangles;
        }

        public void Dispose() { }
        
    }
}