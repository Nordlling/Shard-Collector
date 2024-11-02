using _Main.Scripts.Global.DialogSystem.Services;
using _Main.Scripts.Global.UI.Buttons.Handlers;
using TMPro;
using UnityEngine;

namespace _Main.Scripts.Scenes.Common.UI.Dialogs.Settings
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