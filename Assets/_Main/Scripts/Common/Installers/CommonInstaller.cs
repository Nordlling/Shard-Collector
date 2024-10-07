using System;
using App.Scripts.Modules.Utils.RandomService;
using Main.Scripts.Infrastructure.Services.GameGrid.Loader;
using Main.Scripts.Infrastructure.Services.GameGrid.Parser;
using Zenject;

namespace _Main.Scripts.Common.Installers
{
    public class CommonInstaller : MonoInstaller
    {
		
        public override void InstallBindings()
        {
            Container.Bind<IRandomService>().To<SystemRandomService>().AsSingle().WithArguments(DateTime.Now.Millisecond);
            Container.Bind<ISimpleLoader>().To<SimpleLoader>().AsSingle();
            Container.Bind<ISimpleParser>().To<SimpleParser>().AsSingle();
        }
    }
}