using _Main.Scripts.Common.Dialogs;
using App.Scripts.Modules.Dialogs.Interfaces;
using TMPro;
using UnityEngine;

namespace _Main.Scripts.GameScene.Dialogs
{
    public class SettingsDialog : PoolableDialog
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private IButtonHandler continueButtonHandler;
        [SerializeField] private IButtonHandler mainMenuButtonHandler;

        protected override void OnDialogFinishShow()
        {
            base.OnDialogFinishShow();
            continueButtonHandler.Button.onClick.AddListener(Continue);
            mainMenuButtonHandler.Button.onClick.AddListener(GoToMainMenu);
        }

        protected override void OnDialogReset()
        {
            base.OnDialogReset();
            continueButtonHandler.Button.onClick.RemoveListener(Continue);
            mainMenuButtonHandler.Button.onClick.RemoveListener(GoToMainMenu);
        }

        private void Continue()
        {
            Close(() => continueButtonHandler.Execute());
        }

        private void GoToMainMenu()
        {
            Close(() => mainMenuButtonHandler.Execute());
        }
    }
}