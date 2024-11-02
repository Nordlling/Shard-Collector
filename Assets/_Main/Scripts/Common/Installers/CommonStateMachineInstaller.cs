using _Main.Scripts.Common;
using _Main.Scripts.Common.Installers;
using Main.Scripts.Infrastructure.States;
using Zenject;

namespace Main.Scripts.Infrastructure.Installers.ProjectInstallers
{
    public class CommonStateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
            Container.BindInterfacesTo<TransitSceneState>().AsSingle();
            Container.Bind<IInitializable>().To<CommonInitializator>().AsCached();
        }

    }
}