using System.Collections.Generic;
using _Main.Scripts.Global.Ecs.Extensions;
using _Main.Scripts.Global.GameStateMachine;
using _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.ShapeSelector.Components;
using _Main.Scripts.Scenes.GameScene.GameSceneStates;
using _Main.Scripts.Toolkit.Extensions.Collections;
using _Main.Scripts.Toolkit.Random;
using Scellecs.Morpeh;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.ShapeSelector.Systems
{
    public class ShapeSelectorFillSystem : ISystem
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IRandomService _randomService;
        
        private Filter _shapesFilter;
        private Filter _allShapeInSelectorFilter;
        private Filter _shapesToSelectorFilter;

        private readonly List<Entity> _shapes = new();

        public World World { get; set; }

        public ShapeSelectorFillSystem(IGameStateMachine gameStateMachine, IRandomService randomService)
        {
            _gameStateMachine = gameStateMachine;
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
            
            _shapesToSelectorFilter = World.Filter
                .With<ShapeToSelectorSignal>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_allShapeInSelectorFilter.TryGetFirstEntity(out var entity))
            {
                return;
            }

            ref var allShapesInSelectorSignal = ref entity.GetComponent<AllShapesInSelectorSignal>();

            if (allShapesInSelectorSignal.Delay > 0f)
            {
                allShapesInSelectorSignal.Delay -= deltaTime;
                return;
            }
            
            if (allShapesInSelectorSignal.InMove)
            {
                CheckAllShapesFinishMove(entity);
                return;
            }
            
            StartFillSelector(ref allShapesInSelectorSignal);
        }

        private void CheckAllShapesFinishMove(Entity entity)
        {
            if (_shapesToSelectorFilter.GetLengthSlow() <= 0)
            {
                entity.RemoveComponent<AllShapesInSelectorSignal>();
                _gameStateMachine.Enter<PlayLevelState>();
            }
        }

        private void StartFillSelector(ref AllShapesInSelectorSignal allShapesInSelectorSignal)
        {
            foreach (var shapeEntity in _shapesFilter)
            {
                _shapes.Add(shapeEntity);
            }

            _shapes.ShuffleRandom(_randomService);

            for (int i = 0; i < _shapes.Count; i++)
            {
                var shapeEntity = _shapes[i];
                shapeEntity.SetComponent(new ShapeInSelectorComponent { Index = i });
                shapeEntity.AddComponent<ShapeToSelectorSignal>();
            }

            allShapesInSelectorSignal.InMove = true;
            _shapes.Clear();
        }

        public void Dispose()
        {
        }

    }
}