using _Main.Scripts.Common.InputSystem;
using _Main.Scripts.GameScene;
using Cysharp.Threading.Tasks;

namespace Main.Scripts.Infrastructure.States
{
    public class StartLevelState : IState
    {
        public GameStateMachine StateMachine { get; set; }
        
        private readonly IGameSceneCreator _gameSceneCreator;
        private readonly IInputService _inputService;

        public StartLevelState(IGameSceneCreator gameSceneCreator, IInputService inputService)
        {
            _gameSceneCreator = gameSceneCreator;
            _inputService = inputService;
        }

        public UniTask Enter()
        {
            _inputService.DisableInput();
            _inputService.DisableUIInput();
            _gameSceneCreator.Recreate();
            return UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}