using _Main.Scripts.Global.Ecs.World;
using _Main.Scripts.Global.GameStateMachine;
using _Main.Scripts.Scenes.GameScene.Gameplay.GameBoard.Systems;
using _Main.Scripts.Scenes.GameScene.Gameplay.GameBoard.View;
using _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Components;
using _Main.Scripts.Scenes.GameScene.Services.Layer;
using _Main.Scripts.Scenes.GameScene.Services.Level.CurrentLevel;
using _Main.Scripts.Toolkit.InputSystem;
using Cysharp.Threading.Tasks;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.GameSceneStates
{
    public class StartLevelState : IState
    {
        public GameStateMachine StateMachine { get; set; }
        
        private readonly IWorldRunner _worldRunner;
        private readonly IInputService _inputService;
        private readonly ICurrentLevelService _currentLevelService;
        private readonly GameBoardContent _gameBoardContent;
        private readonly GameBoardInitializer _gameBoardInitializer;
        private readonly ILayerService _layerService;

        public StartLevelState(IWorldRunner worldRunner, IInputService inputService, ICurrentLevelService currentLevelService, 
            GameBoardContent gameBoardContent, GameBoardInitializer gameBoardInitializer, ILayerService layerService)
        {
            _worldRunner = worldRunner;
            _inputService = inputService;
            _currentLevelService = currentLevelService;
            _gameBoardContent = gameBoardContent;
            _gameBoardInitializer = gameBoardInitializer;
            _layerService = layerService;
        }

        public UniTask Enter()
        {
            _inputService.DisableInput();
            _inputService.DisableUIInput();
            StartLevel();
            return UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
        
        private void StartLevel()
        {
            var shapeFilter = _worldRunner.World.Filter.With<ShapeComponent>().Build();
            foreach (var entity in shapeFilter)
            {
                entity.AddComponent<ShapeDestroySignal>();
            }
            
            var patternEntity = _worldRunner.CreateEntity();
            patternEntity.SetComponent(new CreatePatternSignal
            {
                Points = _currentLevelService.CurrentLevel.Points
            });
            patternEntity.SetComponent(new ShapeSpawnSignal
            {
                Parent = _gameBoardContent.PatternContent,
                Size = Vector3.one,
                Position = new Vector3(0f, 0f, 5f)
            });
            
            _layerService.ResetLayers();
            _gameBoardInitializer.Init();
        }
    }
}