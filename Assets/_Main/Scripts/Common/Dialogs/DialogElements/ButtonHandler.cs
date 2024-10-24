using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.Common.Dialogs.DialogElements
{
    public class ButtonHandler : MonoBehaviour
    {
        [SerializeField] private Button button;

        public Button Button => button;
    }
}