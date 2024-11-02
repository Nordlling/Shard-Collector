using _Main.Scripts.Global.ConfigSystem.Level;
using _Main.Scripts.Global.ConfigSystem.Level.Data;
using _Main.Scripts.Scenes.GameScene.Services.Level.CurrentLevel;
using _Main.Scripts.Scenes.GameScene.Services.Level.Status;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Scenes.GameScene.Installers
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