using _Main.Scripts.GameScene.Services;
using App.Scripts.Scenes.Game.Configs.Pool;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.GameScene.MonoInstallers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private LevelConfig levelConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<ILevelLoadService>().To<LevelLoadService>().AsSingle().WithArguments(levelConfig);
            Container.Bind<ICurrentLevelService>().To<CurrentLevelService>().AsSingle().WithArguments(levelConfig);
            Container.Bind<ILevelPlayStatusService>().To<LevelPlayStatusService>().AsSingle();
        }
        
    }
}