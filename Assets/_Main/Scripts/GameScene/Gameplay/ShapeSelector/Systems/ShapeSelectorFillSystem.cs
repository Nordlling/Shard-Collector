using System.Collections.Generic;
using App.Scripts.Modules.EcsWorld.Common.Extensions;
using App.Scripts.Modules.Utils.RandomService;
using DG.Tweening;
using Scellecs.Morpeh;

namespace _Main.Scripts
{
    public class ShapeSelectorFillSystem : ISystem
    {
        private readonly IRandomService _randomService;
        private Filter _shapesFilter;
        private Filter _allShapeInSelectorFilter;
        
        public World World { get; set; }

        public ShapeSelectorFillSystem(IRandomService randomService)
        {
            _randomService = randomService;
        }

        public void OnAwake()
        {
            _allShapeInSelectorFilter = World.Filter
                .With<AllShapesInSelectorSignal>()
                .Build();
            
            _shapesFilter = World.Filter
                .With<ShapeComponent>()
                .Without<PatternMarker>()
                .Without<ShapeDestroySignal>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_allShapeInSelectorFilter.TryGetFirstEntity(out var entity))
            {
                return;
            }

            List<Entity> shapes = new();
            
            foreach (var shapeEntity in _shapesFilter)
            {
                shapes.Add(shapeEntity);
            }

            shapes.ShuffleRandom(_randomService);

            for (int i = 0; i < shapes.Count; i++)
            {
                var shapeEntity = shapes[i];
                shapeEntity.SetComponent(new ShapeInSelectorComponent
                {
                    Index = i
                });
                DOVirtual.DelayedCall(2f, () => shapeEntity.AddComponent<ShapeToSelectorSignal>());
            }

            entity.RemoveComponent<AllShapesInSelectorSignal>();
        }

        public void Dispose()
        {
        }

    }
}