using _Main.Scripts.Gameplay.GameBoard;
using _Main.Scripts.GameScene.MonoInstallers;
using _Main.Scripts.GameScene.Services;
using _Main.Scripts.Spawn;
using App.Scripts.Modules.EcsWorld.Infrastructure.Services;
using App.Scripts.Modules.EcsWorld.Infrastructure.Systems;
using Scellecs.Morpeh;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.GameScene
{
    public class GameSceneStarter : IInitializable, ITickable
    {
        private readonly IWorldRunner _worldRunner;
        private readonly ISystemGroupContainer _systemGroupContainer;
        private readonly ICurrentLevelService _currentLevelService;
        private readonly GameBoardContent _gameBoardContent;
        private readonly GameBoardInitializer _gameBoardInitializer;

        public GameSceneStarter(IWorldRunner worldRunner, ISystemGroupContainer systemGroupContainer, 
            ICurrentLevelService currentLevelService, GameBoardContent gameBoardContent, GameBoardInitializer gameBoardInitializer)
        {
            _worldRunner = worldRunner;
            _systemGroupContainer = systemGroupContainer;
            _currentLevelService = currentLevelService;
            _gameBoardContent = gameBoardContent;
            _gameBoardInitializer = gameBoardInitializer;
        }
        public void StartGameScene()
        {
            _worldRunner.CreateWorld(_systemGroupContainer);
            Create();
        }

        private void Create()
        {
            var patternEntity = _worldRunner.CreateEntity();
            patternEntity.SetComponent(new CreatePatternSignal
            {
                Points = _currentLevelService.GetCurrentLevel().Points
            });
            patternEntity.SetComponent(new ShapeSpawnSignal
            {
                Parent = _gameBoardContent.PatternContent,
                Size = Vector3.one,
                Position = new Vector3(0f, 0f, 5f)
            });
            
            _gameBoardInitializer.Init();
        }

        public void Initialize()
        {
            StartGameScene();
        }

        public void Tick()
        {
            _worldRunner.Update(Time.deltaTime);
        }
    }
}