using _Main.Scripts.Spawn.Services;
using App.Scripts.Scenes.Game.Configs.Pool;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.GameScene.MonoInstallers
{
    public class LoadInstaller : MonoInstaller
    {
        [SerializeField] private LevelConfig levelConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<ILevelLoadService>().To<LevelLoadService>().AsSingle().WithArguments(levelConfig);
        }
        
    }
}