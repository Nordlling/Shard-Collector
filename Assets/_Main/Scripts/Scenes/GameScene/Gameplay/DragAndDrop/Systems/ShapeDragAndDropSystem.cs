using System.Collections.Generic;
using _Main.Scripts.Global.Ecs.Extensions;
using _Main.Scripts.Scenes.GameScene.Gameplay.DragAndDrop.Configs;
using _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Views;
using _Main.Scripts.Scenes.GameScene.Gameplay.ShapeSelector.Components;
using _Main.Scripts.Scenes.GameScene.Services.Level.Status;
using _Main.Scripts.Toolkit.Extensions.Geometry;
using _Main.Scripts.Toolkit.InputSystem;
using _Main.Scripts.Toolkit.Polygon;
using DG.Tweening;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.DragAndDrop.Systems
{
	public class ShapeDragAndDropSystem : ISystem
	{
		private readonly IInputService _inputService;
		private readonly ShapeDragAndDropConfig _shapeDragAndDropConfig;
		private readonly ILevelPlayStatusService _levelPlayStatusService;

		private Filter _shapesFilter;
		private Filter _patternFilter;
		private Filter _shapeSelectorFilter;

		private readonly Dictionary<Vector3, int> _magnetMap = new();
		private readonly Vector3 _maxVector = new(float.MaxValue, float.MaxValue, float.MaxValue);
		
		private Entity _draggedShapeEntity;
		private Vector2 _centerOffset;
		private Vector2 _oldPosition;
		private Vector2 _newPosition;
		
		private bool _dragging;
		private bool _magnet;
		
		public World World { get; set; }

		public ShapeDragAndDropSystem(IInputService inputService, ShapeDragAndDropConfig shapeDragAndDropConfig, ILevelPlayStatusService levelPlayStatusService)
		{
			_inputService = inputService;
			_shapeDragAndDropConfig = shapeDragAndDropConfig;
			_levelPlayStatusService = levelPlayStatusService;
		}

		public void OnAwake()
		{
			_shapesFilter = World.Filter
				.With<ShapeComponent>()
				.Without<PatternMarker>()
				.Build();
			
			_patternFilter = World.Filter
				.With<ShapeComponent>()
				.With<PatternMarker>()
				.Build();
			
			_shapeSelectorFilter = World.Filter
				.With<ShapeSelectorComponent>()
				.Build();
		}

		public void OnUpdate(float deltaTime)
		{
			CheckInput(deltaTime);
		}

		private void CheckInput(float deltaTime)
		{
			if (!_inputService.InputActivity)
			{
				return;
			}
			
			if (_inputService.OnTouchedDown())
			{
				TryTakeShape();
			}
			else if (_inputService.OnTouchedUp())
			{
				TryDropShape(deltaTime);
				_dragging = false;
			}
			else
			{
				TryDragShape(deltaTime);
			}
		}

		private void TryTakeShape()
		{
			if (!_draggedShapeEntity.IsNullOrDisposed() && _draggedShapeEntity.Has<ShapeInMoveMarker>())
			{
				return;
			}
			
			Vector3 touchPosition = _inputService.GetTouchPositionInWorld();

			foreach (var entity in _shapesFilter)
			{
				var shapeView = entity.GetComponent<ShapeComponent>().ShapeView;

				if (!touchPosition.IsInsideMesh(shapeView.MeshFilter.sharedMesh, shapeView.transform))
				{
					continue;
				}
				
				_draggedShapeEntity = entity;
				_oldPosition = shapeView.transform.position;
				_dragging = true;
				_centerOffset = shapeView.transform.position - touchPosition;
				if (_draggedShapeEntity.Has<ShapeInSelectorComponent>())
				{
					_draggedShapeEntity.AddComponent<ShapeFromSelectorSignal>();
				}
				break;
			}
		}

		private void TryDragShape(float deltaTime)
		{
			if (!_dragging)
			{
				return;
			}
			
			Vector2 touchPosition = _inputService.GetTouchPositionInWorld();
			Transform shapeTransform = _draggedShapeEntity.GetComponent<ShapeComponent>().ShapeView.transform;
			Vector2 newShapePosition = touchPosition + _shapeDragAndDropConfig.DragOffset;
			shapeTransform.position = newShapePosition + _centerOffset;
			CheckMagnet(deltaTime);
		}

		private void TryDropShape(float deltaTime)
		{
			if (!_dragging)
			{
				return;
			}

			var shapeView = _draggedShapeEntity.GetComponent<ShapeComponent>().ShapeView;
			shapeView.ShadowTransform.gameObject.SetActive(false);
			bool newShape = !_draggedShapeEntity.Has<ShapeOnPatternMarker>();

			if (!_magnet)
			{
				DropShapeWithoutMagnet(shapeView, newShape);
			}
			else
			{
				_draggedShapeEntity.AddComponent<ShapeInMoveMarker>();
				DropShapeWithMagnet(shapeView, newShape);
			}
		}

		private void DropShapeWithoutMagnet(ShapeView shapeView, bool newShape)
		{
			if (newShape)
			{
				_draggedShapeEntity.AddComponent<ShapeToSelectorSignal>();
			}
			else
			{
				shapeView.transform
					.DOMove(_oldPosition, _shapeDragAndDropConfig.ShapeMoveToPatternDuration)
					.OnKill(() =>
					{
						_draggedShapeEntity.RemoveComponent<ShapeInMoveMarker>();
						_draggedShapeEntity = null;
					});
				
			}
		}

		private void DropShapeWithMagnet(ShapeView shapeView, bool newShape)
		{
			if (newShape)
			{
				_draggedShapeEntity.AddComponent<ShapeOnPatternMarker>();
				_draggedShapeEntity.RemoveComponent<ShapeInSelectorComponent>();
				_shapeSelectorFilter.First().AddComponent<ShapeSelectorResortSignal>();
			}
			
			shapeView.transform
				.DOMove(_newPosition, _shapeDragAndDropConfig.ShapeMoveToPatternDuration)
				.OnKill(() =>
				{
					_draggedShapeEntity.RemoveComponent<ShapeInMoveMarker>();
					_draggedShapeEntity = null;
					_levelPlayStatusService.UseMove(newShape);
				});
		}

		private void CheckMagnet(float deltaTime)
		{
			if (!_patternFilter.TryGetFirstEntity(out var patternEntity))
			{
				return;
			}

			_magnet = false;

			var patternShapeComponent = patternEntity.GetComponent<ShapeComponent>();
			var shapeComponent = _draggedShapeEntity.GetComponent<ShapeComponent>();
			Vector3 shapePosition = shapeComponent.ShapeView.transform.position;
			var shadowTransform = shapeComponent.ShapeView.ShadowTransform;
			var patternView = patternShapeComponent.ShapeView;

			FillMagnetMap(shapeComponent, shapePosition, patternShapeComponent);

			Vector3 minDistance = FindMinDistanceFromMagnetMap(shapePosition, shapeComponent.ExternalPointOffsets,
				patternView.transform.position, patternShapeComponent.ExternalPointOffsets);
			_newPosition = shapePosition + minDistance;
			
			if (minDistance == _maxVector || _newPosition == _oldPosition)
			{
				_magnet = false;
				_newPosition = default;
				Move(shadowTransform, shapePosition, _shapeDragAndDropConfig.ShadowMoveSpeed, deltaTime);
				if (shadowTransform.position == shapePosition)
				{
					shadowTransform.gameObject.SetActive(false);
				}
				return;
			}

			_magnet = true;
			shadowTransform.gameObject.SetActive(true);
			Move(shadowTransform, _newPosition, _shapeDragAndDropConfig.ShadowMoveSpeed, deltaTime);
		}

		private void FillMagnetMap(ShapeComponent shapeComponent, Vector3 shapePosition, ShapeComponent patternShapeComponent)
		{
			_magnetMap.Clear();
			foreach (Vector3 externalOffset in shapeComponent.ExternalPointOffsets)
			{
				Vector3 externalPoint = shapePosition + externalOffset;

				foreach (Vector3 patternPoint in patternShapeComponent.Points)
				{
					Vector3 distanceFromShapeToPattern = patternPoint - externalPoint;
					
					if (distanceFromShapeToPattern.magnitude <= _shapeDragAndDropConfig.MaxOverlapDistance)
					{
						_magnetMap.TryAdd(distanceFromShapeToPattern, 0);
						_magnetMap[distanceFromShapeToPattern]++;
					}
				}
			}
		}

		private Vector3 FindMinDistanceFromMagnetMap(Vector3 shapePosition, List<Vector3> shapeExternalOffsets, Vector3 patternPosition, List<Vector3> patternExternalOffsets)
		{
			Vector3 minDistance = _maxVector;

			foreach (var key in _magnetMap.Keys)
			{
				if (_magnetMap[key] < _shapeDragAndDropConfig.MinOverlapCount || key.magnitude >= minDistance.magnitude)
				{
					continue;
				}

				var newPosition = shapePosition + key;
				if (!ShapeUtils.ShapeInsidePolygon(newPosition, shapeExternalOffsets, patternPosition, patternExternalOffsets))
				{
					continue;
				}
				
				minDistance = key;
			}

			return minDistance;
		}

		private void Move(Transform currentTransform, Vector3 newPosition, float speed, float deltaTime)
		{
			Vector3 currentPosition = currentTransform.position;
			Vector3 direction = newPosition - currentPosition;
			direction.z = 0f;
			float distance = speed * deltaTime;
			if (direction.magnitude < distance)
			{
				currentTransform.position = newPosition;
				return;
			}
			currentPosition += direction.normalized * distance;
			currentTransform.position = currentPosition;
		}

		public void Dispose()
		{
		}
		
	}

}

