using _Main.Scripts.Common.InputSystem;
using Cysharp.Threading.Tasks;

namespace Main.Scripts.Infrastructure.States
{
    public class PlayLevelState : IState
    {
        public GameStateMachine StateMachine { get; set; }
        
        private readonly IInputService _inputService;

        public PlayLevelState(IInputService inputService)
        {
            _inputService = inputService;
        }

        public UniTask Enter()
        {
            _inputService.EnableInput();
            _inputService.EnableUIInput();
            return UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}