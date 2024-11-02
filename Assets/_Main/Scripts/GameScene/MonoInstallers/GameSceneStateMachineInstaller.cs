using _Main.Scripts.Common.Installers;
using Main.Scripts.Infrastructure.States;
using Zenject;

namespace Main.Scripts.Infrastructure.Installers.ProjectInstallers
{
    public class GameSceneStateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<StartGameSceneState>().AsSingle();
            Container.BindInterfacesTo<StartLevelState>().AsSingle();
            Container.BindInterfacesTo<PlayLevelState>().AsSingle();
            Container.BindInterfacesTo<FinishLevelState>().AsSingle();
            Container.Bind<IInitializable>().To<GameSceneInitializator>().AsSingle();
        }

    }
}