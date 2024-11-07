using System;
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Scenes.GameScene.Gameplay.Painter.Views;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Components;
using _Main.Scripts.Toolkit.Polygon;
using Scellecs.Morpeh;
using Sirenix.Utilities;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.DragAndDrop.Systems
{
	public class LayerService : ILayerService
	{
		public event Action<int> OnLayerViewChanged;
		public event Action OnLayersReset;
		public int CurrentLayer { get; private set; }
		public int LayersCount => Layers.Count(el => el != null && el.Count != 0);
		public List<Dictionary<Entity, Vector3[]>> Layers { get; } = new(50);

		public LayerService()
		{
			for (int i = 0; i < Layers.Capacity; i++)
			{
				Layers.Add(new Dictionary<Entity, Vector3[]>());
			}
		}

		public void ChangeLayerView(int layer)
		{
			CurrentLayer = layer;
			OnLayerViewChanged?.Invoke(CurrentLayer);
		}

		public void ResetLayers()
		{
			CurrentLayer = -1;
			for (int i = 0; i < Layers.Capacity; i++)
			{
				Layers[i].Clear();
			}
			OnLayersReset?.Invoke();
		}

		public void FindLayerForShape(Entity shapeEntity)
		{
			FindLayerForShape(shapeEntity, shapeEntity.GetComponent<ShapeComponent>().ShapeView.transform.position);
		}

		public void FindLayerForShape(Entity shapeEntity, Vector3 shapePosition)
		{
			ref var shapeComponent = ref shapeEntity.GetComponent<ShapeComponent>();
			
			var actualPoints = shapeComponent.ExternalPointOffsets;
			for (int i = 0; i < shapeComponent.ExternalPointOffsets.Length; i++)
			{
				actualPoints[i] = shapeComponent.ExternalPointOffsets[i] + shapePosition;
			}

			int layerIndex = -1;
			for (int i = Layers.Count - 1; i >= 0; i--)
			{
				var layer = Layers[i];
				if (layer == null || layer.Count == 0)
				{
					continue;
				}
				bool haveIntersections = layer.Values.Any(layerPoints => PolygonAreaCalculator.DoPolygonsIntersect(layerPoints, actualPoints));
				if (haveIntersections)
				{
					layerIndex = i;
					break;
				}
			}

			layerIndex++;
			Layers[layerIndex].Add(shapeEntity, actualPoints.ToArray());
			shapeComponent.SortingOrder = layerIndex;
			PainterView.Layers = Layers.SelectMany(layer => layer.Values).ToList();
		}

		public void RemoveShapeAndResortLayers(Entity entity)
		{
			Stack<Entity> shapesToResort = new();
			
			for (int i = Layers.Count - 1; i >= 0; i--)
			{
				if (Layers[i] == null || Layers[i].Count == 0)
				{
					continue;
				}
				
				if (Layers[i].ContainsKey(entity))
				{
					Layers[i].Remove(entity);
					break;
				}

				Layers[i].Keys.ForEach(el => shapesToResort.Push(el));
				Layers[i].Clear();
			}
			

			while (shapesToResort.Count > 0)
			{
				var shapeToResort = shapesToResort.Pop();
				FindLayerForShape(shapeToResort);
			}
		}
		
	}
}