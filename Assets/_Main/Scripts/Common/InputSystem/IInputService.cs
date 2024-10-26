using UnityEngine;

namespace _Main.Scripts.Common.InputSystem
{
    public interface IInputService
    {
        bool InputActivity { get; }
        bool UIInputActivity { get; }
        
        void EnableInput();
        void DisableInput();
        void EnableUIInput();
        void DisableUIInput();

        bool OnTouched();
        bool OnTouchedDown();
        bool OnTouchedUp();
        bool OnBack();

        Vector3 GetTouchPositionOnScreen();
        Vector3 GetTouchPositionInWorld();
    }
}