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
    public class GameSceneCreator : IGameSceneCreator, ITickable
    {
        private readonly IWorldRunner _worldRunner;
        private readonly ISystemGroupContainer _systemGroupContainer;
        private readonly ICurrentLevelService _currentLevelService;
        private readonly GameBoardContent _gameBoardContent;
        private readonly GameBoardInitializer _gameBoardInitializer;

        public GameSceneCreator(IWorldRunner worldRunner, ISystemGroupContainer systemGroupContainer, 
            ICurrentLevelService currentLevelService, GameBoardContent gameBoardContent, GameBoardInitializer gameBoardInitializer)
        {
            _worldRunner = worldRunner;
            _systemGroupContainer = systemGroupContainer;
            _currentLevelService = currentLevelService;
            _gameBoardContent = gameBoardContent;
            _gameBoardInitializer = gameBoardInitializer;
        }

        public void Create()
        {
            _worldRunner.CreateWorld(_systemGroupContainer);
            Recreate();
        }

        public void Tick()
        {
            _worldRunner.Update(Time.deltaTime);
        }

        public void DestroyWorld()
        {
            _worldRunner.DestroyWorld();
        }

        public void Recreate()
        {
            var shapeFilter = _worldRunner.World.Filter.With<ShapeComponent>().Build();
            foreach (var entity in shapeFilter)
            {
                entity.AddComponent<ShapeDestroySignal>();
            }
            
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
    }
}