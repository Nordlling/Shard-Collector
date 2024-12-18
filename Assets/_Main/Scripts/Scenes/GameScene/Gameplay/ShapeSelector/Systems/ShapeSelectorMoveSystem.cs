using System;
using _Main.Scripts.Global.Ecs.Extensions;
using _Main.Scripts.Scenes.GameScene.Gameplay.DragAndDrop.Configs;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.ShapeSelector.Components;
using DG.Tweening;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.ShapeSelector.Systems
{
    public class ShapeSelectorMoveSystem : ISystem
    {
        private readonly ShapeDragAndDropConfig _shapeDragAndDropConfig;
        private Filter _shapeSelectorFilter;
        private Filter _shapesToSelectorFilter;
        private Filter _shapesFromSelectorFilter;

        public World World { get; set; }

        public ShapeSelectorMoveSystem(ShapeDragAndDropConfig shapeDragAndDropConfig)
        {
            _shapeDragAndDropConfig = shapeDragAndDropConfig;
        }

        public void OnAwake()
        {
            _shapesToSelectorFilter = World.Filter
                .With<ShapeToSelectorSignal>()
                .With<ShapeInSelectorComponent>()
                .With<ShapeComponent>()
                .Without<ShapeInMoveMarker>()
                .Build();
            
            _shapesFromSelectorFilter = World.Filter
                .With<ShapeFromSelectorSignal>()
                .With<ShapeInSelectorComponent>()
                .With<ShapeComponent>()
                .Build();
            
            _shapeSelectorFilter = World.Filter
                .With<ShapeSelectorComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_shapeSelectorFilter.TryGetFirstEntity(out var shapeSelectorEntity))
            {
                return;
            }

            var shapeSelectorComponent = shapeSelectorEntity.GetComponent<ShapeSelectorComponent>();
            
            foreach (var entity in _shapesToSelectorFilter)
            {
                var shapeComponent = entity.GetComponent<ShapeComponent>();
                var shapeInSelectorComponent = entity.GetComponent<ShapeInSelectorComponent>();

                float firstCellPositionY = shapeSelectorComponent.Rect.yMax - shapeSelectorComponent.PaddingTop -
                                           shapeSelectorComponent.CellOffset - shapeSelectorComponent.CellSize / 2f;

                float targetPositionY = firstCellPositionY - (shapeSelectorComponent.CellOffset * 2f + shapeSelectorComponent.CellSize) * shapeInSelectorComponent.Index;
                float targetPositionX = shapeSelectorComponent.Rect.center.x;
                var targetPosition = new Vector2(targetPositionX, targetPositionY);

                var bounds = shapeComponent.ShapeView.MeshFilter.sharedMesh.bounds;
                var maxSize = Math.Max(bounds.size.x, bounds.size.y);
                var resultSize = Vector3.one * (shapeSelectorComponent.CellSize / maxSize);
                resultSize *= 0.8f;
                
                var shapeTransform = shapeComponent.ShapeView.transform;
                
                entity.AddComponent<ShapeInMoveMarker>();
                DOTween.Sequence()
                    .Append(shapeTransform.DOMove(targetPosition, _shapeDragAndDropConfig.ShapeMoveToSelectorDuration))
                    .Join(shapeTransform.DOScale(resultSize, _shapeDragAndDropConfig.ShapeScaleDuration))
                    .OnComplete(() =>
                    {
                        shapeComponent.SortingOrder = 0;
                        shapeComponent.ShapeView.UpdateSortingOrder(shapeComponent.SortingOrder);
                        entity.RemoveComponent<ShapeToSelectorSignal>();
                        entity.RemoveComponent<ShapeInMoveMarker>();
                    });
            }

            foreach (var entity in _shapesFromSelectorFilter)
            {
                var shapeComponent = entity.GetComponent<ShapeComponent>();
                shapeComponent.ShapeView.transform.DOScale(Vector3.one, 0.3f);
                entity.RemoveComponent<ShapeFromSelectorSignal>();
            }
        }

        public void Dispose()
        {
        }

    }
}