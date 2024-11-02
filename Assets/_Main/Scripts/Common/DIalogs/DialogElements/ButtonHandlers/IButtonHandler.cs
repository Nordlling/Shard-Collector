using UnityEngine.UI;

namespace _Main.Scripts.Common.Dialogs
{
    public interface IButtonHandler
    {
        bool Independent { get; }
        Button Button { get; }
        void Execute();
    }
}