using System;
using System.Collections.Generic;
using _Main.Scripts.Global.Ecs.Extensions;
using _Main.Scripts.Global.GameStateMachine;
using _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.ShapeSelector.Components;
using _Main.Scripts.Scenes.GameScene.GameSceneStates;
using _Main.Scripts.Scenes.GameScene.Services.Level.Status;
using _Main.Scripts.Toolkit.Polygon;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.LevelComplete.Systems
{
    public class LevelCompleteSystem : ISystem
    {
        private readonly PolygonAreaCalculator _polygonAreaCalculator;
        private readonly ILevelPlayStatusService _levelPlayStatusService;
        private readonly IGameStateMachine _gameStateMachine;

        private readonly List<Vector3> _worldPositions = new();
        private readonly List<List<Vector3>> _shapesOfExternalOffsets = new();

        private Filter _shapeInSelectorFilter;
        private Filter _shapeOnPatternMarkerFilter;
        private Filter _patternFilter;
        private bool _canGameOver;

        public World World { get; set; }

        public LevelCompleteSystem(PolygonAreaCalculator polygonAreaCalculator, ILevelPlayStatusService levelPlayStatusService, IGameStateMachine gameStateMachine)
        {
            _polygonAreaCalculator = polygonAreaCalculator;
            _levelPlayStatusService = levelPlayStatusService;
            _gameStateMachine = gameStateMachine;
        }

        public void OnAwake()
        {
            _shapeInSelectorFilter = World.Filter
                .With<ShapeComponent>()
                .With<ShapeInSelectorComponent>()
                .Without<ShapeDestroySignal>()
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
            if (_levelPlayStatusService.LeftMoves > 0 && _levelPlayStatusService.FreeShapes > 0)
            {
                _canGameOver = true;
                return;
            }
            
            if (_canGameOver && (_levelPlayStatusService.LeftMoves <= 0 || _levelPlayStatusService.FreeShapes <= 0))
            {
                GameOver();
                _canGameOver = false;
            }
        }

        private void GameOver()
        {
            if (!_patternFilter.TryGetFirstEntity(out var patternEntity))
            {
                Debug.LogError("No found patternEntity");
                return;
            }
            
            FillShapesInfo();
            var patternArea = patternEntity.GetComponent<ShapeComponent>().Area;
            var placedShapesArea = _polygonAreaCalculator.CalculateUnionArea(_worldPositions, _shapesOfExternalOffsets);
            int percent = (int)Math.Ceiling(placedShapesArea / patternArea * 100f);
            percent = Math.Clamp(percent, 0, 100);
           
            _gameStateMachine.Enter<FinishLevelState, int>(percent);

            Debug.Log($"Left moves = {_levelPlayStatusService.LeftMoves}; Shapes in selector = {_shapeInSelectorFilter.GetLengthSlow()}");
            Debug.Log($"Shapes Area = {placedShapesArea}; Pattern Area = {patternArea}; Percent = {placedShapesArea / patternArea * 100f:F2}%");
        }

        private void FillShapesInfo()
        {
            _worldPositions.Clear();
            _shapesOfExternalOffsets.Clear();
            foreach (var entity in _shapeOnPatternMarkerFilter)
            {
                var shapeComponent = entity.GetComponent<ShapeComponent>();
                _worldPositions.Add(shapeComponent.ShapeView.transform.position);
                _shapesOfExternalOffsets.Add(shapeComponent.ExternalPointOffsets);
            }
        }

        public void Dispose() { }

    }
}