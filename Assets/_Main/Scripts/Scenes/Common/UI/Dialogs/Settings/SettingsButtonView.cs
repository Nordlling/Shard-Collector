using _Main.Scripts.Global.DialogSystem.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Main.Scripts.Scenes.Common.UI.Dialogs.Settings
{
    public class SettingsButtonView : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        private IDialogsService _dialogsService;

        [Inject]
        public void Construct(IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;
        }

        private void OnEnable()
        {
            button.onClick.AddListener(OpenSettings);
        }
        
        private void OnDisable()
        {
            button.onClick.RemoveListener(OpenSettings);
        }

        private void OpenSettings()
        {
            var dialog = _dialogsService.GetDialog<SettingsDialog>();
            _dialogsService.ShowDialog(dialog);
        }
        
    }
}