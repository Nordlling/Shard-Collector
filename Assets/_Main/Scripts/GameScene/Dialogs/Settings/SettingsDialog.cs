using _Main.Scripts.Common.Dialogs.DialogElements;
using App.Scripts.Modules.Dialogs.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Main.Scripts.GameScene.Dialogs
{
    public class SettingsDialog : PoolableDialog
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private ButtonHandler continueButton;
        [SerializeField] private ButtonHandler mainMenuButton;
        
        private IDialogsService _dialogsService;

        [Inject]
        public void Construct(IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;
        }

        protected override void OnDialogShown()
        {
            base.OnDialogShown();
            continueButton.Button.onClick.AddListener(Continue);
            mainMenuButton.Button.onClick.AddListener(GoToMainMenu);
        }

        protected override void OnDialogReset()
        {
            base.OnDialogReset();
            continueButton.Button.onClick.RemoveListener(Continue);
            mainMenuButton.Button.onClick.RemoveListener(GoToMainMenu);
        }

        private void Continue()
        {
            Close(null);
        }

        private void GoToMainMenu()
        {
            Close(null);
            SceneManager.LoadScene("MainScene");
        }
    }
}