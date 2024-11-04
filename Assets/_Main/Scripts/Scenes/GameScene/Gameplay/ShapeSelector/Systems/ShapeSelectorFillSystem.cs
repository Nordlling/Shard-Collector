using System.Collections.Generic;
using _Main.Scripts.Global.Ecs.Extensions;
using _Main.Scripts.Global.GameStateMachine;
using _Main.Scripts.Scenes.GameScene.Gameplay.DragAndDrop.Configs;
using _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.ShapeSelector.Components;
using _Main.Scripts.Scenes.GameScene.GameSceneStates;
using _Main.Scripts.Toolkit.Extensions.Collections;
using _Main.Scripts.Toolkit.Random;
using DG.Tweening;
using Scellecs.Morpeh;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.ShapeSelector.Systems
{
    public class ShapeSelectorFillSystem : ISystem
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IRandomService _randomService;
        private readonly ShapeDragAndDropConfig _shapeDragAndDropConfig;
        
        private Filter _shapesFilter;
        private Filter _allShapeInSelectorFilter;
        private Filter _shapesInMoveFilter;

        private readonly List<Entity> _shapes = new();
        private bool _shapesInMove;
        private bool _inProgress;

        public World World { get; set; }

        public ShapeSelectorFillSystem(IGameStateMachine gameStateMachine, IRandomService randomService, ShapeDragAndDropConfig shapeDragAndDropConfig)
        {
            _gameStateMachine = gameStateMachine;
            _randomService = randomService;
            _shapeDragAndDropConfig = shapeDragAndDropConfig;
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
            
            _shapesInMoveFilter = World.Filter
                .With<ShapeToSelectorSignal>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_inProgress || !_allShapeInSelectorFilter.TryGetFirstEntity(out var entity))
            {
                return;
            }

            if (_shapesInMove)
            {
                if (_shapesInMoveFilter.GetLengthSlow() <= 0)
                {
                    entity.RemoveComponent<AllShapesInSelectorSignal>();
                    _shapesInMove = false;
                    _gameStateMachine.Enter<PlayLevelState>();
                }

                return;
            }

            _shapes.Clear();
            
            foreach (var shapeEntity in _shapesFilter)
            {
                _shapes.Add(shapeEntity);
            }

            _shapes.ShuffleRandom(_randomService);

            for (int i = 0; i < _shapes.Count; i++)
            {
                var shapeEntity = _shapes[i];
                shapeEntity.SetComponent(new ShapeInSelectorComponent { Index = i });
                _inProgress = true;
                DOVirtual.DelayedCall(_shapeDragAndDropConfig.StartLevelShapeMoveToPatternDelay, () =>
                {
                    _inProgress = false;
                    _shapesInMove = true;
                    shapeEntity.AddComponent<ShapeToSelectorSignal>();
                });
            }
        }

        public void Dispose()
        {
        }

    }
}