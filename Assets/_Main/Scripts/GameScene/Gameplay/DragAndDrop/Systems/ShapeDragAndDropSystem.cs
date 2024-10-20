using UnityEngine;
using System.Collections.Generic;
using App.Scripts.Modules.EcsWorld.Common.Extensions;
using Scellecs.Morpeh;

namespace _Main.Scripts 
{
	public class ShapeDragAndDropSystem : ISystem
	{
		private readonly Camera _camera;
		private readonly ShapeDragAndDropConfig _shapeDragAndDropConfig;
		
		private Filter _shapesFilter;
		private Filter _patternFilter;
		private Filter _shapeSelectorFilter;

		private bool _dragging;
		private bool _magnet;
		private Entity _draggedShapeEntity;
		private Vector2 _centerOffset;
		private readonly Dictionary<Vector3, int> _magnetMap = new();
		private Vector2 _newPosition;

		public World World { get; set; }

		public ShapeDragAndDropSystem(Camera mainCamera, ShapeDragAndDropConfig shapeDragAndDropConfig)
		{
			_camera = mainCamera;
			_shapeDragAndDropConfig = shapeDragAndDropConfig;
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
			if (Input.GetMouseButtonDown(0))
			{
				TryTakeShape();
			}
			else if (Input.GetMouseButtonUp(0))
			{
				TryDropShape();
				_dragging = false;
				_draggedShapeEntity = null;
			}

			TryDragShape();
		}

		private void TryTakeShape()
		{
			Vector3 screen = Input.mousePosition;
			screen.z = Mathf.Abs(_camera.transform.position.z - 0f);
			Vector3 mousePosition = _camera.ScreenToWorldPoint(screen);
			Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);

			foreach (var entity in _shapesFilter)
			{
				var shapeComponent = entity.GetComponent<ShapeComponent>();
				var shapeView = shapeComponent.ShapeView;
				if (mousePosition2D.IsInsideMesh(shapeView.MeshFilter.mesh, shapeView.transform))
				{
					_draggedShapeEntity = entity;
					_dragging = true;
					_centerOffset = shapeComponent.ShapeView.transform.position - mousePosition;
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
			
			Vector3 screen = Input.mousePosition;
			screen.z = Mathf.Abs(_camera.transform.position.z - 0f);
			Vector3 mousePosition = _camera.ScreenToWorldPoint(screen);
			Transform shapeTransform = _draggedShapeEntity.GetComponent<ShapeComponent>().ShapeView.transform;
			Vector2 newShapePosition = new Vector3(mousePosition.x, mousePosition.y, shapeTransform.position.z);
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
			shapeView.transform.position = _magnet ? _newPosition : shapeView.transform.position;
			shapeView.ShadowTransform.gameObject.SetActive(false);

			if (_magnet)
			{
				_draggedShapeEntity.AddComponent<ShapeOnPatternSignal>();
			}
			
			if (!_draggedShapeEntity.Has<ShapeInSelectorComponent>())
			{
				return;
			}
			
			if (_magnet)
			{
				_draggedShapeEntity.TryAddComponent<ShapeOnPatternMarker>();
				_draggedShapeEntity.RemoveComponent<ShapeInSelectorComponent>();
				if (_shapeSelectorFilter.TryGetFirstEntity(out var shapeSelectorEntity))
				{
					shapeSelectorEntity.AddComponent<ShapeSelectorResortSignal>();
				}
			}
			else
			{
				_draggedShapeEntity.AddComponent<ShapeToSelectorSignal>();
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

