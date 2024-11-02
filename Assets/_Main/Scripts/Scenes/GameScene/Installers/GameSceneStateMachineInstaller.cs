using _Main.Scripts.Scenes.GameScene.GameSceneStates;
using _Main.Scripts.Scenes.GameScene.Initialization;
using Zenject;

namespace _Main.Scripts.Scenes.GameScene.Installers
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