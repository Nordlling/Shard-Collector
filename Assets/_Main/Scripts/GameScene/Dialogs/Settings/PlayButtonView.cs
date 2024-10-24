using App.Scripts.Modules.Dialogs.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace _Main.Scripts.GameScene.Dialogs
{
    public class PlayButtonView : MonoBehaviour
    {
        [SerializeField] private Button button;
        

        [Inject]
        public void Construct()
        {
            
        }

        private void OnEnable()
        {
            button.onClick.AddListener(OpenGameScene);
        }
        
        private void OnDisable()
        {
            button.onClick.RemoveListener(OpenGameScene);
        }

        private void OpenGameScene()
        {
            SceneManager.LoadScene("GameScene");
        }
        
    }
}