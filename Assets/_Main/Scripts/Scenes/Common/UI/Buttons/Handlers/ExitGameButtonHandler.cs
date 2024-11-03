using System.Collections.Generic;
using _Main.Scripts.Global.GameStateMachine;
using _Main.Scripts.Toolkit.EditorTools.Scene;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.Global.UI.Buttons.Handlers
{
    public class ExitGameButtonHandler : MonoBehaviour, IButtonHandler
    {
        [SerializeField] private bool independent = true;
        [SerializeField] private Button button;
        
        private IGameStateMachine _gameStateMachine;
        
        public Button Button => button;
        public bool Independent => independent;

        private void OnEnable()
        {
            if (independent)
            {
                button.onClick.AddListener(ExitGame);
            }
        }

        private void OnDisable()
        {
            if (independent)
            {
                button.onClick.RemoveListener(ExitGame);
            }
        }
        
        public void Execute()
        {
            ExitGame();
        }

        private void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            UnityEngine.Application.Quit();
#endif
        }
        
        private List<string> GetSceneNames()
        {
            return ScenesHelper.GetSceneNames();
        }
        
    }
}