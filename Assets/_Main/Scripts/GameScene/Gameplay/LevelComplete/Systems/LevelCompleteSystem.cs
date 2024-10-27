using System;
using System.Collections.Generic;
using _Main.Scripts.Gameplay.GameBoard;
using _Main.Scripts.GameScene.Dialogs;
using _Main.Scripts.GameScene.Services;
using App.Scripts.Modules.Dialogs.Interfaces;
using App.Scripts.Modules.EcsWorld.Common.Extensions;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts
{
    public class LevelCompleteSystem : ISystem
    {
        private readonly ICurrentLevelService _currentLevelService;
        private readonly IDialogsService _dialogsService;
        private readonly PolygonAreaCalculator _polygonAreaCalculator;
        
        private readonly List<Vector3> _worldPositions = new();
        private readonly List<List<Vector3>> _shapesOfExternalOffsets = new();

        private Filter _createPatternSignal;
        private Filter _shapeInSelectorFilter;
        private Filter _shapeOnPatternSignalFilter;
        private Filter _shapeOnPatternMarkerFilter;
        private Filter _patternFilter;

        private int _leftMoves;

        public World World { get; set; }

        public LevelCompleteSystem(ICurrentLevelService currentLevelService, IDialogsService dialogsService, PolygonAreaCalculator polygonAreaCalculator)
        {
            _currentLevelService = currentLevelService;
            _dialogsService = dialogsService;
            _polygonAreaCalculator = polygonAreaCalculator;
        }

        public void OnAwake()
        {
            _createPatternSignal = World.Filter
                .With<CreatePatternSignal>()
                .Build();
            
            _shapeInSelectorFilter = World.Filter
                .With<ShapeComponent>()
                .With<ShapeInSelectorComponent>()
                .Without<ShapeDestroySignal>()
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
            
            _currentLevelService.LevelUp();

            FillShapesInfo();

            var patternArea = patternEntity.GetComponent<ShapeComponent>().Area;
            var placedShapesArea = _polygonAreaCalculator.CalculateUnionArea(_worldPositions, _shapesOfExternalOffsets);
            
            var percent = Math.Ceiling(placedShapesArea / patternArea * 100f);
           
            OpenLevelCompleteDialog(percent);

            Debug.Log($"Left moves = {_leftMoves}; Shapes in selector = {_shapeInSelectorFilter.GetLengthSlow()}");
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

        private void OpenLevelCompleteDialog(double percent)
        {
            var dialog = _dialogsService.GetDialog<LevelCompleteDialog>();
            dialog.Setup((int)percent);
            _dialogsService.ShowDialog(dialog);
        }

        public void Dispose()
        {
        }

    }
}