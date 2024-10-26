using UnityEngine;
using System.Collections.Generic;
using _Main.Scripts.Common.InputSystem;
using _Main.Scripts.Gameplay.GameBoard;
using _Main.Scripts.Gameplay.Painter;
using _Main.Scripts.GameScene.MonoInstallers;
using _Main.Scripts.GameScene.Services;
using _Main.Scripts.Pattern;
using _Main.Scripts.Spawn;
using App.Scripts.Modules.EcsWorld.Common.Extensions;
using Scellecs.Morpeh;

namespace _Main.Scripts 
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

		private bool _dragging;
		private List<Vector2> _points = new();
		private bool _moveSwitcher;

		public World World { get; set; }

		public PainterSystem(IInputService inputService, PatternDrawingConfig patternDrawingConfig, 
			PainterView painterView, ICurrentLevelService currentLevelService, GameBoardContent gameBoardContent)
		{
			_inputService = inputService;
			_patternDrawingConfig = patternDrawingConfig;
			_painterView = painterView;
			_currentLevelService = currentLevelService;
			_gameBoardContent = gameBoardContent;
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
			if (_inputService.InputActivity)
			{
				if (Input.GetMouseButtonDown(1)) {
					_dragging = true;
					_points.Clear();
				} else if(Input.GetMouseButtonUp(1)) {
					_dragging = false;
					CreatePattern();
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
				shapeSelectorEntity.AddComponent<AllShapesInSelectorSignal>();
			}
		}
		
		public void Dispose()
		{
		}
		
	}
}