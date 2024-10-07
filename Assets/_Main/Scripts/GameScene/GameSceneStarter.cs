using _Main.Scripts.Gameplay.GameBoard;
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
        private readonly ILevelLoadService _levelLoadService;

        public GameSceneStarter(IWorldRunner worldRunner, ISystemGroupContainer systemGroupContainer, 
            ILevelLoadService levelLoadService)
        {
            _worldRunner = worldRunner;
            _systemGroupContainer = systemGroupContainer;
            _levelLoadService = levelLoadService;
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
                Points = _levelLoadService.GetDefaultLevel().Points
            });
            patternEntity.SetComponent(new ShapeSpawnSignal
            {
                Size = Vector3.one,
                Position = new Vector3(0f, 0f, 5f)
            });
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