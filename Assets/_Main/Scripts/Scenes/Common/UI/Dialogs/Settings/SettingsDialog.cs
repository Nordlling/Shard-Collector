using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Global.DialogSystem.Services;
using _Main.Scripts.Global.UI.Buttons.Handlers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Main.Scripts.Scenes.Common.UI.Dialogs.Settings
{
    public class SettingsDialog : PoolableDialog
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Dictionary<string, IButtonHandler[]> buttonHandlersByScene;
        
        private string _currentSceneName;

        private void DeactivateAllButtonHandlers()
        {
            foreach (var handlers in buttonHandlersByScene)
            {
                foreach (var handler in handlers.Value)
                {
                    handler.Button.gameObject.SetActive(false);
                }
            }
        }

        protected override void OnDialogSetup()
        {
            DeactivateAllButtonHandlers();
            
            _currentSceneName = SceneManager.GetActiveScene().name;
           
            if (!buttonHandlersByScene.TryGetValue(_currentSceneName, out var buttonHandlers))
            {
                return;
            }
            
            foreach (var buttonHandler in buttonHandlers)
            {
                buttonHandler.Button.gameObject.SetActive(true);
                buttonHandler.Button.onClick.AddListener(() => HandleClick(buttonHandler));
            }
        }
        
        protected override void OnDialogReset()
        {
            base.OnDialogReset();
            
            foreach (var buttonHandler in buttonHandlersByScene[_currentSceneName])
            {
                buttonHandler.Button.onClick.RemoveListener(() => HandleClick(buttonHandler));
            }
        }

        private void HandleClick(IButtonHandler buttonHandler)
        {
            Close(() => buttonHandler.Execute());
        }
        
    }
}