using System;
using App.Scripts.Modules.EcsWorld.Common.Extensions;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts
{
    public class ShapeSelectorMoveSystem : ISystem
    {
        private Filter _shapeSelectorFilter;
        private Filter _shapesToSelectorFilter;
        private Filter _shapesFromSelectorFilter;

        public World World { get; set; }

        public void OnAwake()
        {
            _shapesToSelectorFilter = World.Filter
                .With<ShapeToSelectorSignal>()
                .With<ShapeInSelectorComponent>()
                .With<ShapeComponent>()
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

                var shapeTransform = shapeComponent.ShapeView.transform;
                shapeTransform.position = targetPosition;

                var bounds = shapeComponent.ShapeView.MeshFilter.mesh.bounds;
                var maxSize = Math.Max(bounds.size.x, bounds.size.y);

                var resultSize = Vector3.one * (shapeSelectorComponent.CellSize / maxSize);

                resultSize *= 0.8f;
                
                shapeTransform.localScale = resultSize;

                entity.RemoveComponent<ShapeToSelectorSignal>();
            }

            foreach (var entity in _shapesFromSelectorFilter)
            {
                var shapeComponent = entity.GetComponent<ShapeComponent>();
                shapeComponent.ShapeView.transform.localScale = new Vector3(1f, 1f, 1f);
                entity.RemoveComponent<ShapeFromSelectorSignal>();
            }
        }

        public void Dispose()
        {
        }

    }
}