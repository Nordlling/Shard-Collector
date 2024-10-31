using System;
using System.Collections.Generic;
using _Main.Scripts.GameScene;
using _Main.Scripts.GameScene.MonoInstallers;
using _Main.Scripts.Pattern;
using _Main.Scripts.Spawn;
using _Main.Scripts.Toolkit;
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
        private readonly ShapeGrouper _shapeGrouper;
        private readonly ILevelPlayStatusService _levelPlayStatusService;

        private Filter _createPatternShapesFilter;

        public World World { get; set; }

        public PatternSpawnSystem(IPool<ShapeView> pool, GenerateConfig generateConfig, 
            RenderConfig renderConfig, IRandomService randomService, GameBoardContent gameBoardContent, 
            PolygonAreaCalculator polygonAreaCalculator, ShapeGrouper shapeGrouper, ILevelPlayStatusService levelPlayStatusService)
        {
            _pool = pool;
            _generateConfig = generateConfig;
            _renderConfig = renderConfig;
            _randomService = randomService;
            _gameBoardContent = gameBoardContent;
            _polygonAreaCalculator = polygonAreaCalculator;
            _shapeGrouper = shapeGrouper;
            _levelPlayStatusService = levelPlayStatusService;
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
                var shapeComponent = patternEntity.GetComponent<ShapeComponent>();
                var minWeight = shapeComponent.Area * _generateConfig.MinShapeAreaFraction;
                var maxWeight = shapeComponent.Area * _generateConfig.MaxShapeAreaFraction;
                CreateShapesByPattern(shapeComponent.Triangles, minWeight, maxWeight);
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
            
            patternView.Init(patternEntity, _renderConfig.PatternMaterial);
            // patternEntity.AddComponent<ShapeRenderMarker>();
            return true;
        }

        private void CreateShapesByPattern(Triangle2D[] patternTriangles, double minWeight, double maxWeight)
        {
            var shapes = _shapeGrouper.GroupTrianglesIntoShapes(patternTriangles, minWeight, maxWeight);
            _levelPlayStatusService.InitNewLevel(shapes.Count);

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

        public void Dispose() { }
        
    }
}