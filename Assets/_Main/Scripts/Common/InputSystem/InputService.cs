using UnityEngine;
using UnityEngine.EventSystems;

namespace _Main.Scripts.Common.InputSystem
{
    public class InputService : IInputService
    {
        private readonly EventSystem _eventSystem;
        private readonly Camera _camera;

        public InputService(EventSystem eventSystem, Camera camera)
        {
            _eventSystem = eventSystem;
            _camera = camera;
            InputActivity = true;
        }

        public bool InputActivity { get; private set; }
        public bool UIInputActivity => _eventSystem.enabled;
        
        public void EnableInput() => InputActivity = true;
        public void DisableInput() => InputActivity = false;
        public void EnableUIInput() => _eventSystem.enabled = true;
        public void DisableUIInput() => _eventSystem.enabled = false;
        
        public bool OnTouched() => Input.GetMouseButton(0);
        public bool OnTouchedDown() =>  Input.GetMouseButtonDown(0);
        public bool OnTouchedUp() => Input.GetMouseButtonUp(0);
        public bool OnBack() => Input.GetKeyUp(KeyCode.Escape);

        public Vector3 GetTouchPositionOnScreen()
        {
            return Input.mousePosition;
        }
        
        public Vector3 GetTouchPositionInWorld()
        {
            Vector2 mousePositionOnScreen = Input.mousePosition;
            return _camera.ScreenToWorldPoint(mousePositionOnScreen);
        }

    }
}