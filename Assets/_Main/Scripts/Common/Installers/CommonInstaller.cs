using System;
using _Main.Scripts.GameScene.Services;
using _Main.Scripts.Toolkit.File;
using App.Scripts.Modules.Utils.RandomService;
using Zenject;

namespace _Main.Scripts.Common.Installers
{
    public class CommonInstaller : MonoInstaller
    {
		
        public override void InstallBindings()
        {
            Container.Bind<IRandomService>().To<SystemRandomService>().AsSingle().WithArguments(DateTime.Now.Millisecond);
            Container.Bind<ISaveKeysContainer>().To<SaveKeysContainer>().AsSingle();
            Container.Bind<ISimpleLoader>().To<SimpleLoader>().AsSingle();
            Container.Bind<ISimpleParser>().To<SimpleParser>().AsSingle();
            Container.Bind<IStorageService>().To<StoragePrefsService>().AsSingle();
            Container.Bind<ILevelSaveService>().To<LevelSaveService>().AsSingle();
        }
    }
}