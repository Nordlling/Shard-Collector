using _Main.Scripts.GameScene.Dialogs;
using _Main.Scripts.GameScene.Services;
using App.Scripts.Modules.Dialogs.Interfaces;
using Cysharp.Threading.Tasks;

namespace Main.Scripts.Infrastructure.States
{
    public class FinishLevelState : IParametrizedState<int>
    {
        private readonly IDialogsService _dialogsService;
        private readonly ICurrentLevelService _currentLevelService;
        public GameStateMachine StateMachine { get; set; }

        public FinishLevelState(IDialogsService dialogsService, ICurrentLevelService currentLevelService)
        {
            _dialogsService = dialogsService;
            _currentLevelService = currentLevelService;
        }

        public UniTask Enter(int percent)
        {
            _currentLevelService.FinishLevel(percent);
            _currentLevelService.TryLevelUp();
            OpenLevelCompleteDialog();
            return UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
        
        private void OpenLevelCompleteDialog()
        {
            var dialog = _dialogsService.GetDialog<LevelCompleteDialog>();
            _dialogsService.ShowDialog(dialog);
        }
    }
}