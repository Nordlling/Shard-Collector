using _Main.Scripts.Gameplay.GameBoard;
using App.Scripts.Modules.EcsWorld.Infrastructure.Services;
using App.Scripts.Modules.EcsWorld.Infrastructure.Systems;
using mattatz.Utils;
using Scellecs.Morpeh;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Spawn
{
    public class GameSceneStarter : IInitializable, ITickable
    {
        private readonly IWorldRunner _worldRunner;
        private readonly ISystemGroupContainer _systemGroupContainer;

        public GameSceneStarter(IWorldRunner worldRunner, ISystemGroupContainer systemGroupContainer)
        {
            _worldRunner = worldRunner;
            _systemGroupContainer = systemGroupContainer;
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
                Points = LocalStorage.LoadList<Vector2>("points2.json"),
            });
            patternEntity.SetComponent(new ShapeSpawnSignal
            {
                Size = Vector3.one,
                Position = new Vector3(0f, 0f, 5f)
            });
            
            var painterEntity = _worldRunner.CreateEntity();
            patternEntity.SetComponent(new CreatePatternSignal
            {
                Points = LocalStorage.LoadList<Vector2>("points2.json"),
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