using System;
using System.Collections.Generic;
using _Main.Scripts.GameScene;
using _Main.Scripts.GameScene.Dialogs;
using _Main.Scripts.GameScene.Services;
using App.Scripts.Modules.Dialogs.Interfaces;
using App.Scripts.Modules.EcsWorld.Infrastructure.Services;
using Scellecs.Morpeh;
using UnityEngine;
using Zenject;

namespace _Main.Scripts
{
    public class LevelCompleteService : ILevelCompleteService, IInitializable
    {
        private readonly IWorldRunner _worldRunner;
        private readonly ICurrentLevelService _currentLevelService;
        private readonly ILevelPlayStatusService _levelPlayStatusService;
        private readonly IDialogsService _dialogsService;
        private readonly PolygonAreaCalculator _polygonAreaCalculator;

        private readonly List<Vector3> _worldPositions = new();
        private readonly List<List<Vector3>> _shapesOfExternalOffsets = new();

        private Filter _shapeOnPatternMarkerFilter;
        private Filter _patternFilter;

        public LevelCompleteService(IWorldRunner worldRunner, 
            ICurrentLevelService currentLevelService, ILevelPlayStatusService levelPlayStatusService, 
            IDialogsService dialogsService, PolygonAreaCalculator polygonAreaCalculator)
        {
            _worldRunner = worldRunner;
            _currentLevelService = currentLevelService;
            _levelPlayStatusService = levelPlayStatusService;
            _dialogsService = dialogsService;
            _polygonAreaCalculator = polygonAreaCalculator;
        }

        public void Initialize()
        {
            _shapeOnPatternMarkerFilter = _worldRunner.World.Filter
                .With<ShapeComponent>()
                .With<ShapeOnPatternMarker>()
                .Build();
            
            _patternFilter = _worldRunner.World.Filter
                .With<ShapeComponent>()
                .With<PatternMarker>()
                .Build();

            _levelPlayStatusService.OnUsedMove += TryGameOver;
        }

        private void TryGameOver()
        {
            if (_levelPlayStatusService.LeftMoves > 0 && _levelPlayStatusService.FreeShapes > 0)
            {
                return;
            }
            
            Entity patternEntity = _patternFilter.First();
            
            _currentLevelService.LevelUp();
            FillShapesInfo();

            var patternArea = patternEntity.GetComponent<ShapeComponent>().Area;
            var placedShapesArea = _polygonAreaCalculator.CalculateUnionArea(_worldPositions, _shapesOfExternalOffsets);
            
            var percent = Math.Ceiling(placedShapesArea / patternArea * 100f);
           
            OpenLevelCompleteDialog(percent);

            Debug.Log($"Left moves = {_levelPlayStatusService.LeftMoves}; Shapes in selector = {_levelPlayStatusService.FreeShapes}");
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

    }
}