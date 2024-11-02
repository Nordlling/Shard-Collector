using _Main.Scripts.GameScene;
using Cysharp.Threading.Tasks;

namespace Main.Scripts.Infrastructure.States
{
    public class StartGameSceneState : IState
    {
        public GameStateMachine StateMachine { get; set; }
        
        private readonly IGameSceneCreator _gameSceneCreator;

        public StartGameSceneState(IGameSceneCreator gameSceneCreator)
        {
            _gameSceneCreator = gameSceneCreator;
        }

        public async UniTask Enter()
        {
            _gameSceneCreator.CreateWorld();
            await StateMachine.Enter<StartLevelState>();
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}