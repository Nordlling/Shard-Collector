using _Main.Scripts.Scenes.GameScene.GameSceneStates;
using _Main.Scripts.Scenes.GameScene.Initialization;
using Zenject;

namespace _Main.Scripts.Scenes.GameScene.Installers
{
    public class MainSceneStateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<StartMainSceneState>().AsSingle();
            Container.Bind<IInitializable>().To<MainSceneInitializator>().AsSingle();
        }

    }
}