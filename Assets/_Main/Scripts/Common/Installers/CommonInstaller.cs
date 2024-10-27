using System;
using _Main.Scripts.GameScene.Services;
using _Main.Scripts.Toolkit.File;
using App.Scripts.Modules.Utils.RandomService;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Common.Installers
{
    public class CommonInstaller : MonoInstaller
    {
        [SerializeField] private bool setSeed;
        [SerializeField] [ShowIf(nameof(setSeed))] private int seed;
        
        
        public override void InstallBindings()
        {
            int currentSeed = setSeed ? seed : DateTime.Now.Millisecond;
            Container.Bind<IRandomService>().To<SystemRandomService>().AsSingle().WithArguments(currentSeed);
            UnityEngine.Random.InitState(currentSeed);

            Container.Bind<ISaveKeysContainer>().To<SaveKeysContainer>().AsSingle();
            Container.Bind<ISimpleLoader>().To<SimpleLoader>().AsSingle();
            Container.Bind<ISimpleParser>().To<SimpleParser>().AsSingle();
            Container.Bind<IStorageService>().To<StoragePrefsService>().AsSingle();
            Container.Bind<ILevelSaveService>().To<LevelSaveService>().AsSingle();
        }
    }
}