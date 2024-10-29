using UnityEngine;
using System.Collections.Generic;
using _Main.Scripts.Common.InputSystem;
using App.Scripts.Modules.EcsWorld.Common.Extensions;
using Scellecs.Morpeh;

namespace _Main.Scripts.GameScene
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
				// .With<ShapeInSelectorComponent>()
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
			CheckInput();
			TryDragShape();
		}

		private void CheckInput()
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
				TryDropShape();
				_dragging = false;
				_draggedShapeEntity = null;
			}
		}

		private void TryTakeShape()
		{
			Vector3 touchPosition = _inputService.GetTouchPositionInWorld();

			foreach (var entity in _shapesFilter)
			{
				var shapeComponent = entity.GetComponent<ShapeComponent>();
				var shapeView = shapeComponent.ShapeView;
				if (touchPosition.IsInsideMesh(shapeView.MeshFilter.mesh, shapeView.transform))
				{
					_draggedShapeEntity = entity;
					_oldPosition = shapeView.transform.position;
					_dragging = true;
					_centerOffset = shapeComponent.ShapeView.transform.position - touchPosition;
					if (_draggedShapeEntity.Has<ShapeInSelectorComponent>())
					{
						_draggedShapeEntity.AddComponent<ShapeFromSelectorSignal>();
					}
					break;
				}
			}
		}

		private void TryDragShape()
		{
			if (!_dragging)
			{
				return;
			}
			
			Vector3 touchPosition = _inputService.GetTouchPositionInWorld();
			Transform shapeTransform = _draggedShapeEntity.GetComponent<ShapeComponent>().ShapeView.transform;
			Vector2 newShapePosition = new Vector3(touchPosition.x, touchPosition.y, shapeTransform.position.z);
			shapeTransform.position = newShapePosition + _centerOffset;
			CheckMagnet();
		}

		private void TryDropShape()
		{
			if (!_dragging)
			{
				return;
			}

			var shapeView = _draggedShapeEntity.GetComponent<ShapeComponent>().ShapeView;
			shapeView.ShadowTransform.gameObject.SetActive(false);
			bool newShape = !_draggedShapeEntity.Has<ShapeOnPatternMarker>();

			if (_magnet)
			{
				shapeView.transform.position = _newPosition;
				// _draggedShapeEntity.AddComponent<ShapeOnPatternSignal>();
				
				if (newShape)
				{
					_draggedShapeEntity.AddComponent<ShapeOnPatternMarker>();
					_draggedShapeEntity.RemoveComponent<ShapeInSelectorComponent>();
					_shapeSelectorFilter.First().AddComponent<ShapeSelectorResortSignal>();
				}
				_levelPlayStatusService.UseMove(newShape);
			}
			else
			{
				shapeView.transform.position = _oldPosition;
				if (newShape)
				{
					_draggedShapeEntity.AddComponent<ShapeToSelectorSignal>();
				}
			}
		}

		private void CheckMagnet()
		{
			if (!_patternFilter.TryGetFirstEntity(out var patternEntity))
			{
				return;
			}

			_magnet = false;
			_magnetMap.Clear();

			var patternShapeComponent = patternEntity.GetComponent<ShapeComponent>();
			var shapeComponent = _draggedShapeEntity.GetComponent<ShapeComponent>();
			Vector3 shapePosition = shapeComponent.ShapeView.transform.position;
			var shadowTransform = shapeComponent.ShapeView.ShadowTransform;
			shadowTransform.gameObject.SetActive(false);

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
			
			Vector3 minDistance = FindMinDistanceFromMagnetMap();

			_newPosition = shapePosition + minDistance;
			
			_magnet = minDistance != default;

			if (!_magnet)
			{
				return;
			}
			
			if (!ShapeInsidePattern(_newPosition, shapeComponent.ExternalPointOffsets, patternShapeComponent))
			{
				_magnet = false;
				return;
			}

			shadowTransform.gameObject.SetActive(true);
			shadowTransform.position = new Vector3(_newPosition.x, _newPosition.y, shadowTransform.position.z);
		}

		private Vector3 FindMinDistanceFromMagnetMap()
		{
			Vector3 minDistance = default;

			foreach (var key in _magnetMap.Keys)
			{
				if (_magnetMap[key] < _shapeDragAndDropConfig.MinOverlapCount)
				{
					continue;
				}

				if (minDistance == default || key.magnitude < minDistance.magnitude)
				{
					minDistance = key;
				}
			}

			return minDistance;
		}

		private bool ShapeInsidePattern(Vector3 shapeNewPosition, List<Vector3> shapeExternalOffsets, ShapeComponent patternShapeComponent)
		{
			foreach (var externalOffset in shapeExternalOffsets)
			{
				Vector3 externalPoint = shapeNewPosition + externalOffset;
				if (!PointInPattern(externalPoint, patternShapeComponent.ExternalPointOffsets, patternShapeComponent.ShapeView.transform.position))
				{
					return false;
				}
			}
			
			return true;
		}

		private bool PointInPattern(Vector3 point, List<Vector3> patternOffsets, Vector3 patternPosition) 
		{
			bool result = false;
			var length = patternOffsets.Count;
			for(int i = 0, j = length - 1; i < length; j = i++)
			{
				var position = patternPosition + patternOffsets[i];
				var previousPosition = patternPosition + patternOffsets[j];
				
				if (position == point)
				{
					return true;
				}
				if (((position.y >= point.y) != (previousPosition.y >= point.y)) && 
				    (point.x <= (previousPosition.x - position.x) * (point.y - position.y) / (previousPosition.y - position.y) + position.x)) 
				{
					result = !result;
				}
			}
			return result;
		}

		public void Dispose()
		{
		}
		
	}

}

