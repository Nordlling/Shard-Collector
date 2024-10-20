using System.Collections.Generic;
using _Main.Scripts.Gameplay.GameBoard;
using _Main.Scripts.GameScene.Services;
using App.Scripts.Modules.EcsWorld.Common.Extensions;
using Scellecs.Morpeh;
using UnityEngine;
using Clipper2Lib;

namespace _Main.Scripts
{
    public class LevelCompleteSystem : ISystem
    {
        private readonly ICurrentLevelService _currentLevelService;
        private Filter _createPatternSignal;
        private Filter _shapeInSelectorFilter;
        private Filter _shapeOnPatternSignalFilter;
        private Filter _shapeOnPatternMarkerFilter;
        private Filter _patternFilter;

        private int _leftMoves;

        public World World { get; set; }

        public LevelCompleteSystem(ICurrentLevelService currentLevelService)
        {
            _currentLevelService = currentLevelService;
        }

        public void OnAwake()
        {
            _createPatternSignal = World.Filter
                .With<CreatePatternSignal>()
                .Build();
            
            _shapeInSelectorFilter = World.Filter
                .With<ShapeComponent>()
                .With<ShapeInSelectorComponent>()
                .Build();
            
            _shapeOnPatternSignalFilter = World.Filter
                .With<ShapeComponent>()
                .With<ShapeOnPatternSignal>()
                .Build();
            
            _shapeOnPatternMarkerFilter = World.Filter
                .With<ShapeComponent>()
                .With<ShapeOnPatternMarker>()
                .Build();
            
            _patternFilter = World.Filter
                .With<ShapeComponent>()
                .With<PatternMarker>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_createPatternSignal.TryGetFirstEntity(out _))
            {
                _leftMoves = _shapeInSelectorFilter.GetLengthSlow() + _currentLevelService.GetCurrentLevel().ExtraMoves;
            }
            
            if (!_shapeOnPatternSignalFilter.TryGetFirstEntity(out var signal))
            {
                return;
            }
            
            _leftMoves--;
            signal.RemoveComponent<ShapeOnPatternSignal>();
            
            if (_leftMoves <= 0 || _shapeInSelectorFilter.FirstOrDefault() == default)
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            if (!_patternFilter.TryGetFirstEntity(out var patternEntity))
            {
                Debug.LogError("No found patternEntity");
                return;
            }

            var patternArea = CalculateArea(patternEntity.GetComponent<ShapeComponent>().ExternalPointOffsets);
            var placedShapesArea = CalculateUnionArea(_shapeOnPatternMarkerFilter);
            
            Debug.Log("1________________GAME IS OVER___________________1");
            Debug.Log($"Left moves = {_leftMoves}; Shapes in selector = {_shapeInSelectorFilter.GetLengthSlow()}");
            Debug.Log($"Pattern Area = {patternArea}; Shapes Area = {placedShapesArea}; Percent = {placedShapesArea / patternArea * 100f:F2}%");
            Debug.Log("2________________GAME IS OVER___________________2");
        }
        
        private double CalculateArea(List<Vector3> vertices, FillRule fillRule = FillRule.NonZero)
        {
            ClipperD clipper = new ClipperD();
            
            PathD path = new PathD();
            foreach (var vertex in vertices)
            {
                path.Add(new PointD(vertex.x, vertex.y));
            }

            clipper.AddSubject(path);

            var solution = new PathsD();
            clipper.Execute(ClipType.Union, fillRule, solution);

            double totalArea = Clipper.Area(solution);
            return totalArea;
        }

        private double CalculateUnionArea(Filter shapeFilter, FillRule fillRule = FillRule.NonZero, bool combineByTriangles = true)
        {
            const double scaleFactor = 1;
            
            ClipperD clipper = new ClipperD();
            PathsD paths = new PathsD { new PathD() };

            foreach (var entity in shapeFilter)
            {
                var shapeComponent = entity.GetComponent<ShapeComponent>();
                
                if (combineByTriangles)
                {
                    CombineByTriangles(clipper, paths, shapeComponent, scaleFactor);
                }
                else
                {
                    CombineByExternalPoints(clipper, paths, shapeComponent, scaleFactor);
                }

            }

            clipper.Execute(ClipType.Union, fillRule, paths);
            

            double totalArea = Clipper.Area(paths) / (scaleFactor * scaleFactor);
            return totalArea;
        }

        private void CombineByTriangles(ClipperD clipper, PathsD paths, ShapeComponent shapeComponent, double scaleFactor)
        {
            var shapePosition = shapeComponent.ShapeView.transform.position;
            foreach (var triangle in shapeComponent.Triangles)
            {
                paths[0].Clear();
                paths[0].Add(CreatePoint(shapePosition, triangle.a.Coordinate, scaleFactor));
                paths[0].Add(CreatePoint(shapePosition, triangle.b.Coordinate, scaleFactor));
                paths[0].Add(CreatePoint(shapePosition, triangle.c.Coordinate, scaleFactor));
                clipper.AddSubject(paths);
            }
        }

        private void CombineByExternalPoints(ClipperD clipper, PathsD paths, ShapeComponent shapeComponent, double scaleFactor)
        {
            var shapePosition = shapeComponent.ShapeView.transform.position;
            paths[0].Clear();
            foreach (var offset in shapeComponent.ExternalPointOffsets)
            {
                paths[0].Add(CreatePoint(shapePosition, offset, scaleFactor));
            }
            clipper.AddSubject(paths);
        }

        private PointD CreatePoint(Vector3 shapePosition, Vector2 point, double scaleFactor)
        {
            return new PointD((shapePosition.x + point.x) * scaleFactor, (shapePosition.y + point.y) * scaleFactor);
        }
        
        public void Dispose()
        {
        }

    }
}