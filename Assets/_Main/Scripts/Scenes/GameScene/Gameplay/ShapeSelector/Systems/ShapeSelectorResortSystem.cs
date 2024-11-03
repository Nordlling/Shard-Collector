using System.Collections.Generic;
using _Main.Scripts.Global.Ecs.Extensions;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.ShapeSelector.Components;
using Scellecs.Morpeh;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.ShapeSelector.Systems
{
    public class ShapeSelectorResortSystem : ISystem
    {
        private Filter _shapeSelectorFilter;
        private Filter _shapesInSelectorFilter;

        private readonly List<int> _visibleIndexes = new();

        public World World { get; set; }

        public void OnAwake()
        {
            _shapeSelectorFilter = World.Filter
                .With<ShapeSelectorComponent>()
                .With<ShapeSelectorResortSignal>()
                .Build();
            
            _shapesInSelectorFilter = World.Filter
                .With<ShapeInSelectorComponent>()
                .With<ShapeComponent>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_shapeSelectorFilter.TryGetFirstEntity(out var shapeSelectorEntity))
            {
                return;
            }

            var shapeSelectorComponent = shapeSelectorEntity.GetComponent<ShapeSelectorComponent>();
            
            _visibleIndexes.Clear();

            for (int i = 0; i < shapeSelectorComponent.MaxCount; i++)
            {
                _visibleIndexes.Add(i);
            }
            
            foreach (var entity in _shapesInSelectorFilter)
            {
                var shapeInSelectorComponent = entity.GetComponent<ShapeInSelectorComponent>();

                if (shapeInSelectorComponent.Index < shapeSelectorComponent.MaxCount)
                {
                    _visibleIndexes.Remove(shapeInSelectorComponent.Index);
                }
                
            }
            
            foreach (var entity in _shapesInSelectorFilter)
            {
                if (_visibleIndexes.Count == 0)
                {
                    break;
                }
                
                ref var shapeInSelectorComponent = ref entity.GetComponent<ShapeInSelectorComponent>();

                if (shapeInSelectorComponent.Index >= shapeSelectorComponent.MaxCount)
                {
                    shapeInSelectorComponent.Index = _visibleIndexes[0];
                    entity.AddComponent<ShapeToSelectorSignal>();
                    entity.AddComponent<ShapeInMoveMarker>();
                    _visibleIndexes.RemoveAt(0);
                }
                
            }

            shapeSelectorEntity.RemoveComponent<ShapeSelectorResortSignal>();
        }

        public void Dispose()
        {
        }

    }
}