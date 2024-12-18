using _Main.Scripts.Global.GameStateMachine;
using _Main.Scripts.Global.GameStates;
using Zenject;

namespace _Main.Scripts.Global.Installers
{
    public class GlobalStateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameStateMachine>().To<GameStateMachine.GameStateMachine>().AsSingle();
            Container.BindInterfacesTo<TransitSceneState>().AsSingle();
        }

    }
}