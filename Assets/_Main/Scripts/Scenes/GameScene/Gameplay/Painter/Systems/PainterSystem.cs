using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Global.Ecs.Extensions;
using _Main.Scripts.Scenes.GameScene.Gameplay.DragAndDrop.Configs;
using _Main.Scripts.Scenes.GameScene.Gameplay.GameBoard.View;
using _Main.Scripts.Scenes.GameScene.Gameplay.Painter.Views;
using _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Configs;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.ShapeSelector.Components;
using _Main.Scripts.Scenes.GameScene.Services.Layer;
using _Main.Scripts.Scenes.GameScene.Services.Level.CurrentLevel;
using _Main.Scripts.Toolkit.InputSystem;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.Painter.Systems 
{
	// for test
	public class PainterSystem : ISystem
	{
		private Filter _shapeFilter;
		private Filter _shapeSelectorFilter;

		private readonly IInputService _inputService;
		private readonly PatternDrawingConfig _patternDrawingConfig;
		private readonly PainterView _painterView;
		private readonly ICurrentLevelService _currentLevelService;
		private readonly GameBoardContent _gameBoardContent;
		private readonly ILayerService _layerService;
		private readonly ShapeDragAndDropConfig _shapeDragAndDropConfig;

		private bool _dragging;
		private List<Vector2> _points = new();
		private string _pointsText; // for debug
		private bool _moveSwitcher;

		public World World { get; set; }

		public PainterSystem(IInputService inputService, PatternDrawingConfig patternDrawingConfig,
			PainterView painterView, ICurrentLevelService currentLevelService, GameBoardContent gameBoardContent,
			ILayerService layerService, ShapeDragAndDropConfig shapeDragAndDropConfig)
		{
			_inputService = inputService;
			_patternDrawingConfig = patternDrawingConfig;
			_painterView = painterView;
			_currentLevelService = currentLevelService;
			_gameBoardContent = gameBoardContent;
			_layerService = layerService;
			_shapeDragAndDropConfig = shapeDragAndDropConfig;
		}

		public void OnAwake()
		{
			_painterView.SetPoints(_points);
			
			_shapeFilter = World.Filter
				.With<ShapeComponent>()
				.Build();
			
			_shapeSelectorFilter = World.Filter
				.With<ShapeSelectorComponent>()
				.Build();
		}

		public void OnUpdate(float deltaTime)
		{
			if (_inputService.InputActivity && _patternDrawingConfig.Enabled)
			{
				if (Input.GetMouseButtonDown(0)) {
					_dragging = true;
					_points.Clear();
				} else if(Input.GetMouseButtonUp(0)) {
					_dragging = false;
					CreatePattern();
					_pointsText = string.Join(",", _points.Select(el => $"\"x\":{el.x}, \"y\":{el.y}").ToList());
					_patternDrawingConfig.Enabled = false;
					_layerService.ResetLayers();
				}
			}

			if (_dragging)
			{
				var touchPosition = _inputService.GetTouchPositionInWorld();
				if (_points.Count <= 0 || Vector2.Distance(touchPosition, _points[^1]) > _patternDrawingConfig.Threshold) 
				{
					_points.Add(touchPosition);
				}
			}
		}

		private void CreatePattern()
		{
			foreach (var entity in _shapeFilter)
			{
				entity.AddComponent<ShapeDestroySignal>();
			}
			
			Entity patternEntity = World.CreateEntity();
			patternEntity.SetComponent(new CreatePatternSignal { Points = _points });
			patternEntity.SetComponent(new ShapeSpawnSignal
			{
				Parent = _gameBoardContent.PatternContent,
				Size = Vector3.one
			});

			if (_shapeSelectorFilter.TryGetFirstEntity(out var shapeSelectorEntity))
			{
				shapeSelectorEntity.SetComponent(new AllShapesInSelectorSignal
				{
					Delay = _shapeDragAndDropConfig.StartLevelShapeMoveToPatternDelay
				});
			}
		}
		
		public void Dispose()
		{
		}
		
	}
}