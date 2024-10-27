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
        private readonly GenerateConfig _generateConfig;
        private readonly RenderConfig _renderConfig;
        private readonly IRandomService _randomService;
        private readonly GameBoardContent _gameBoardContent;
        private readonly PolygonAreaCalculator _polygonAreaCalculator;

        private Filter _createPatternShapesFilter;

        public World World { get; set; }

        public PatternSpawnSystem(IPool<ShapeView> pool, GenerateConfig generateConfig, 
            RenderConfig renderConfig, IRandomService randomService, GameBoardContent gameBoardContent, 
            PolygonAreaCalculator polygonAreaCalculator)
        {
            _pool = pool;
            _generateConfig = generateConfig;
            _renderConfig = renderConfig;
            _randomService = randomService;
            _gameBoardContent = gameBoardContent;
            _polygonAreaCalculator = polygonAreaCalculator;
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
            
            if (TryCreatePattern(patternEntity))
            {
                CreateShapesByPattern(patternEntity.GetComponent<ShapeComponent>().Triangles);
            }
        }

        private bool TryCreatePattern(Entity patternEntity)
        {
            ref var patternSignal = ref patternEntity.GetComponent<CreatePatternSignal>();
            ref var shapeSignal = ref patternEntity.GetComponent<ShapeSpawnSignal>();

            var points = patternSignal.Points;
            if (points.Count < 2)
            {
                return false;
            }

            Polygon2D polygon = Polygon2D.Contour(points.ToArray());
            Vertex2D[] vertices = polygon.Vertices;

            if (vertices.Length < 3)
            {
                return false;
            }

            Triangulation2D triangulation;
            try
            {
                triangulation = new Triangulation2D(polygon, _generateConfig.Angle);
                triangulation.SortVerticesClockwise();
            }
            catch (Exception)
            {
                return false;
            }

            Mesh mesh = triangulation.Build();
            
            ShapeView patternView = _pool.Get();
            patternView.Init(patternEntity, _renderConfig.PatternMaterial);
            shapeSignal.Position.z = 1f;
            patternView.SetupTransformProperties(shapeSignal.Parent, shapeSignal.Position, shapeSignal.Size);
            patternView.MeshFilter.sharedMesh = mesh;
            
            List<Vector3> externalPoints = ShapeUtils.FindExternalPoints(triangulation.Triangles, patternView.transform.position);
            double area = _polygonAreaCalculator.CalculateArea(externalPoints);
            
            ShapeComponent patternShapeComponent = new ShapeComponent
            {
                ShapeView = patternView,
                Area = area,
                Points = mesh.vertices,
                ExternalPointOffsets = externalPoints,
                Triangles = triangulation.Triangles
            };
            
            patternEntity.SetComponent(patternShapeComponent);
            patternEntity.AddComponent<PatternMarker>();
            // patternEntity.AddComponent<ShapeRenderMarker>();
            return true;
        }

        private void CreateShapesByPattern(Triangle2D[] triangles)
        {
            var activeTriangles = new List<Triangle2D>();
            var allTriangles = new List<Triangle2D>(triangles);
            var shapes = new List<List<Triangle2D>>();
			
            int shapesCount = _randomService.Range(_generateConfig.MinShapesCount, _generateConfig.MaxShapesCount);
            shapesCount = Math.Min(shapesCount, allTriangles.Count);
            int allTrianglesCount = allTriangles.Count;

            while (allTriangles.Count != 0)
            {
                List<Triangle2D> usedTriangles = new();
                
                int firstTriangleIndex = _randomService.Range(0, allTriangles.Count);
                Triangle2D firstTriangle = allTriangles[firstTriangleIndex];
                activeTriangles.Add(firstTriangle);
                
                int maxCount = _randomService.Range(1, allTrianglesCount / shapesCount);
                maxCount = Math.Min(maxCount, allTriangles.Count);
				
                CreateShape(allTriangles, usedTriangles, activeTriangles, maxCount);
				
                shapes.Add(usedTriangles);
                allTriangles.RemoveAll(el => usedTriangles.Contains(el));
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
        
        private void CreateShape(List<Triangle2D> allTriangles, List<Triangle2D> usedTriangles, List<Triangle2D> activeTriangles, int trianglesCountLeft)
        {
            List<Triangle2D> newActiveTriangles = new();
			
            usedTriangles.AddRange(activeTriangles);
			
            if (trianglesCountLeft == 0 || activeTriangles.Count == 0)
            {
                activeTriangles.Clear();
                return;
            }
			
            activeTriangles.ShuffleRandom(_randomService);

            foreach (Triangle2D activeTriangle in activeTriangles)
            {
                if (trianglesCountLeft <= 0)
                {
                    break;
                }
				
                List<Triangle2D> adjacentTriangles = FindAdjacentTriangles(allTriangles, activeTriangle);

                if (adjacentTriangles.Count == 0)
                {
                    continue;
                }

                adjacentTriangles.ShuffleRandom(_randomService);

                int maxAdjacentCount = Math.Min(adjacentTriangles.Count, trianglesCountLeft);
                int adjacentCount = _randomService.Range(1, maxAdjacentCount + 1);

                newActiveTriangles.AddRange(adjacentTriangles.GetRange(0, adjacentCount));
                trianglesCountLeft -= adjacentCount;
            }
			
            activeTriangles.Clear();
            activeTriangles.AddRange(newActiveTriangles);

            CreateShape(allTriangles, usedTriangles, activeTriangles, trianglesCountLeft);
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