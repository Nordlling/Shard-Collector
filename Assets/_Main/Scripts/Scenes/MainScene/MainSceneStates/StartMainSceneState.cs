using _Main.Scripts.Global.GameStateMachine;
using Cysharp.Threading.Tasks;

namespace _Main.Scripts.Scenes.GameScene.GameSceneStates
{
    public class StartMainSceneState : IState
    {
        public GameStateMachine StateMachine { get; set; }

        public UniTask Enter()
        {
            return UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}