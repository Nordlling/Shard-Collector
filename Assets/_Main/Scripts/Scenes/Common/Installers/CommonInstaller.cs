using System;
using _Main.Scripts.Global.SaveSystem.Level;
using _Main.Scripts.Global.UI.Loading;
using _Main.Scripts.Toolkit.File.Loader;
using _Main.Scripts.Toolkit.File.Parser;
using _Main.Scripts.Toolkit.File.Saver;
using _Main.Scripts.Toolkit.Random;
using _Main.Scripts.Toolkit.Scene;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Global.Installers
{
    public class CommonInstaller : MonoInstaller
    {
        [SerializeField] private LoadingScreen loadingScreen;
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

            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<LoadingScreen>().FromInstance(loadingScreen).AsSingle();
        }
    }
}