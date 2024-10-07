using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Gameplay.GameBoard;
using _Main.Scripts.Gameplay.Painter;
using _Main.Scripts.Pattern;
using _Main.Scripts.Spawn;
using mattatz.Utils;
using Scellecs.Morpeh;

namespace _Main.Scripts 
{
	public class PainterSystem : ISystem
	{
		private Filter _shapeFilter;
		private Filter _patternFilter;

		private readonly Camera _mainCamera;
		private readonly PatternDrawingConfig _patternDrawingConfig;
		private readonly PainterView _painterView;
		private readonly ILevelLoadService _levelLoadService;

		private bool _dragging;
		private List<Vector2> _points = new();
		private bool _moveSwitcher;

		public World World { get; set; }

		public PainterSystem(Camera mainCamera, PatternDrawingConfig patternDrawingConfig, 
			PainterView painterView, ILevelLoadService levelLoadService)
		{
			_mainCamera = mainCamera;
			_patternDrawingConfig = patternDrawingConfig;
			_painterView = painterView;
			_levelLoadService = levelLoadService;
		}

		public void OnAwake()
		{
			_painterView.SetPoints(_points);
			
			_shapeFilter = World.Filter
				.With<ShapeComponent>()
				.Without<PatternMarker>()
				.Build();
			
			_patternFilter = World.Filter
				.With<ShapeComponent>()
				.With<PatternMarker>()
				.Build();
		}

		public void OnUpdate(float deltaTime)
		{
			if (Input.GetMouseButtonDown(0)) {
				_dragging = true;
				_points.Clear();
			} else if(Input.GetMouseButtonUp(0)) {
				_dragging = false;
				CreatePattern();
			}
			else if (Input.GetKeyUp(KeyCode.F))
			{
				_points = _levelLoadService.GetDefaultLevel().Points;
				CreatePattern();
			}

			if (Input.GetKeyDown(KeyCode.Space))
			{
				SwitchMoveAllShapes();
			}

			if (_dragging) {
				var inputPositionOnScreen = Input.mousePosition;
				var inputPositionInWorld = _mainCamera.ScreenToWorldPoint(inputPositionOnScreen);
				if (_points.Count <= 0 || Vector2.Distance(inputPositionInWorld, _points[^1]) > _patternDrawingConfig.Threshold) 
				{
					_points.Add(inputPositionInWorld);
				}
			}
		}

		private void CreatePattern()
		{
			foreach (var entity in _shapeFilter)
			{
				entity.AddComponent<ShapeDestroySignal>();
			}
			
			foreach (var entity in _patternFilter)
			{
				entity.AddComponent<ShapeDestroySignal>();
			}
			
			Entity patternEntity = World.CreateEntity();
			patternEntity.SetComponent(new CreatePatternSignal { Points = _points });
			patternEntity.SetComponent(new ShapeSpawnSignal { Size = Vector3.one });
		}

		private void SwitchMoveAllShapes()
		{
			_moveSwitcher = !_moveSwitcher;
			foreach (var shapeEntity in _shapeFilter)
			{
				var shapeComponent = shapeEntity.GetComponent<ShapeComponent>();
				if (_moveSwitcher)
				{
					shapeComponent.ShapeView.ShapeRigidbody.velocity = Vector3.zero;
					shapeComponent.ShapeView.ShapeRigidbody.angularVelocity = Vector3.zero;
				}
				else
				{
					Vector3 direction = UnityEngine.Random.insideUnitSphere.normalized;
					direction.z = 0f;
					shapeComponent.ShapeView.ShapeRigidbody.AddForce(direction * UnityEngine.Random.Range(50f, 100f));
				}
			}
		}

		
		public void Dispose()
		{
		}
		
	}
}