using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.DragAndDrop.Systems
{
    public interface ILayerService
    {
        event Action<int> OnLayerViewChanged;
        event Action OnLayersReset;
        int CurrentLayer { get; }
        int LayersCount { get; }
        List<Dictionary<Entity, Vector3[]>> Layers { get; }
        void ChangeLayerView(int layer);
        void ResetLayers();
        void FindLayerForShape(Entity shapeEntity);
        void FindLayerForShape(Entity shapeEntity, Vector3 shapePosition);
        void RemoveShapeAndResortLayers(Entity entity);
    }
}