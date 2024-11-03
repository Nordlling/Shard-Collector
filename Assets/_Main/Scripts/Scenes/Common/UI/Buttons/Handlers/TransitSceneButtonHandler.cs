using System.Collections.Generic;
using _Main.Scripts.Global.GameStateMachine;
using _Main.Scripts.Global.GameStates;
using _Main.Scripts.Toolkit.EditorTools.Scene;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Main.Scripts.Global.UI.Buttons.Handlers
{
    public class TransitSceneButtonHandler : MonoBehaviour, IButtonHandler
    {
        [SerializeField] private bool independent = true;
        [SerializeField] private Button button;
        [SerializeField, ValueDropdown(nameof(GetSceneNames))] private string transitSceneName;
        
        private IGameStateMachine _gameStateMachine;
        
        public Button Button => button;
        public bool Independent => independent;

        [Inject]
        public void Construct(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void OnEnable()
        {
            if (independent)
            {
                button.onClick.AddListener(TransitScene);
            }
        }

        private void OnDisable()
        {
            if (independent)
            {
                button.onClick.RemoveListener(TransitScene);
            }
        }
        
        public void Execute()
        {
            TransitScene();
        }

        private void TransitScene()
        {
            _gameStateMachine.Enter<TransitSceneState, string>(transitSceneName);
        }
        
        private List<string> GetSceneNames()
        {
            return ScenesHelper.GetSceneNames();
        }
        
    }
}