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
		
		private bool _dragging;
		private ShapeComponent _draggedShapeComponent;
		private Vector2 _centerOffset;

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
				.Without<PatternMarker>()
				.Build();
			
			_patternFilter = World.Filter
				.With<ShapeComponent>()
				.With<PatternMarker>()
				.Build();
		}

		public void OnUpdate(float deltaTime)
		{
			if (Input.GetMouseButtonDown(1))
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
						_draggedShapeComponent = shapeComponent;
						_dragging = true;
						_centerOffset = shapeComponent.ShapeView.transform.position - mousePosition;
						break;
					}
				}
				
			}
			else if (Input.GetMouseButtonUp(1)) 
			{
				_dragging = false;
			}

			if (_dragging) {
				Vector3 screen = Input.mousePosition;
				screen.z = Mathf.Abs(_camera.transform.position.z - 0f);
				Vector3 mousePosition = _camera.ScreenToWorldPoint(screen);
				Transform shapeTransform = _draggedShapeComponent.ShapeView.transform;
				Vector2 newShapePosition = new Vector3(mousePosition.x, mousePosition.y, shapeTransform.position.z);
				shapeTransform.position = newShapePosition + _centerOffset;
				CheckMagnet();
			}
		}

		private void CheckMagnet()
		{
			if (!_patternFilter.TryGetFirstEntity(out var shapesMap))
			{
				return;
			}

			var shapesMapPoints = shapesMap.GetComponent<ShapeComponent>().Points;
			Dictionary<Vector2, int> checker = new();
			Vector3 shapePosition = _draggedShapeComponent.ShapeView.transform.position;
			
			foreach (Vector3 externalOffset in _draggedShapeComponent.ExternalPoints)
			{
				Vector3 externalPoint = shapePosition + externalOffset;
				foreach (Vector3 mapPoint in shapesMapPoints)
				{
					if (Vector2.Distance(externalPoint, mapPoint) <= _shapeDragAndDropConfig.MinOverlapDistance)
					{
						Vector2 distance = mapPoint - externalPoint;
						checker.TryAdd(distance, 0);
						checker[distance]++;
						break;
					}
				}
			}

			if (checker.Count == 0)
			{
				return;
			}

			var maxEntry = checker.Aggregate((max, next) => next.Value > max.Value ? next : max);
			if (maxEntry.Value >= _shapeDragAndDropConfig.MinOverlapCount)
			{
				_draggedShapeComponent.ShapeView.transform.position += new Vector3(maxEntry.Key.x, maxEntry.Key.y, 0f);
			}
		}

		public void Dispose()
		{
		}
	}

}

