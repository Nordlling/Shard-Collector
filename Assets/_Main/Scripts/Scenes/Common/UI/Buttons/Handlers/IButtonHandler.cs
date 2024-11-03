using UnityEngine.UI;

namespace _Main.Scripts.Global.UI.Buttons.Handlers
{
    public interface IButtonHandler
    {
        bool Independent { get; }
        Button Button { get; }
        void Execute();
    }
}