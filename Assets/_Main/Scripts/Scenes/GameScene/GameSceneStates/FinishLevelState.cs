using _Main.Scripts.Global.DialogSystem.Services;
using _Main.Scripts.Global.GameStateMachine;
using _Main.Scripts.Scenes.GameScene.Services.Level.CurrentLevel;
using _Main.Scripts.Scenes.GameScene.UI.Dialogs.LevelComplete;
using Cysharp.Threading.Tasks;

namespace _Main.Scripts.Scenes.GameScene.GameSceneStates
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