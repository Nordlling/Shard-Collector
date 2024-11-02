using _Main.Scripts.Global.Ecs.World;
using _Main.Scripts.Global.GameStateMachine;
using Cysharp.Threading.Tasks;

namespace _Main.Scripts.Scenes.GameScene.GameSceneStates
{
    public class StartGameSceneState : IState
    {
        private readonly IWorldRunner _worldRunner;
        private readonly ISystemGroupContainer _systemGroupContainer;
        public GameStateMachine StateMachine { get; set; }

        public StartGameSceneState(IWorldRunner worldRunner, ISystemGroupContainer systemGroupContainer)
        {
            _worldRunner = worldRunner;
            _systemGroupContainer = systemGroupContainer;
        }

        public async UniTask Enter()
        {
            _worldRunner.CreateWorld(_systemGroupContainer);
            await StateMachine.Enter<StartLevelState>();
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}