using UnityEngine;
using System.Linq;
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
			if (Input.GetMouseButtonDown(1))
			{
				TryTakeShape();
			}
			else if (Input.GetMouseButtonUp(1))
			{
				TryDropShape();
				_dragging = false;
			}

			if (_dragging)
			{
				DragShape();
			}
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

		private void DragShape()
		{
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
			if (!_dragging || !_draggedShapeEntity.Has<ShapeInSelectorComponent>())
			{
				return;
			}
			
			if (_magnet)
			{
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

			Vector3[] patternPoints = patternEntity.GetComponent<ShapeComponent>().Points;
			
			ShapeComponent shapeComponent = _draggedShapeEntity.GetComponent<ShapeComponent>();
			Vector3 shapePosition = shapeComponent.ShapeView.transform.position;

			foreach (Vector3 externalOffset in shapeComponent.ExternalPointOffsets)
			{
				Vector3 externalPoint = shapePosition + externalOffset;
				foreach (Vector3 patternPoint in patternPoints)
				{
					Vector3 distanceFromShapeToPattern = patternPoint - externalPoint;
					
					if (distanceFromShapeToPattern.magnitude <= _shapeDragAndDropConfig.MinOverlapDistance)
					{
						_magnetMap.TryAdd(distanceFromShapeToPattern, 0);
						_magnetMap[distanceFromShapeToPattern]++;
						break;
					}
				}
			}

			if (_magnetMap.Count == 0)
			{
				return;
			}

			var maxEntry = _magnetMap.Aggregate((max, next) => next.Value > max.Value ? next : max);
			_magnet = maxEntry.Value >= _shapeDragAndDropConfig.MinOverlapCount;
			if (_magnet)
			{
				shapeComponent.ShapeView.transform.position += maxEntry.Key;
			}
		}

		public void Dispose()
		{
		}
	}

}

