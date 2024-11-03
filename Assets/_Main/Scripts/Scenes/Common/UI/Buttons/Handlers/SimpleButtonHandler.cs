using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.Global.UI.Buttons.Handlers
{
    public class SimpleButtonHandler : MonoBehaviour, IButtonHandler
    {
        [SerializeField] private Button button;

        public bool Independent => false;
        public Button Button => button;
        public void Execute() { }
    }
}