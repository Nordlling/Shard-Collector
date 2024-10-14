using UnityEngine;

namespace _Main.Scripts.Toolkit.Screen
{
    public class ScreenService : IScreenService
    {
        private readonly Camera _camera;

        public ScreenService(Camera camera)
        {
            _camera = camera;
        }

        public Rect CalculateRect(float relativePositionX, float relativePositionY, float relativeWidth, float relativeHeight)
        {
            var screenCenter = 0f;
            
            var screenHeight = _camera.orthographicSize * 2;
            var halfScreenHeight = screenHeight / 2f;

            var screenWidth = screenHeight * _camera.aspect;
            var halfScreenWidth = screenWidth / 2f;
            
            var rectCenterPositionX = (screenCenter - halfScreenWidth) + (screenWidth * relativePositionX);
            var rectCenterPositionY = (screenCenter - halfScreenHeight) + (screenHeight * relativePositionY);

            var rectSize = new Vector2(relativeWidth * screenWidth, relativeHeight * screenHeight);
            var rectPosition = new Vector2(rectCenterPositionX - (rectSize.x / 2f), rectCenterPositionY - (rectSize.y / 2f));

            var rect = new Rect(rectPosition, rectSize);
            return rect;
        }

        public Vector2 ConvertRelativeSizeToWorld(Vector2 relativeSize)
        {
            var screenHeight = _camera.orthographicSize * 2;
            var screenWidth = screenHeight * _camera.aspect;
            
            return new Vector2(relativeSize.x * screenWidth, relativeSize.y * screenHeight);
        }

        public Vector2 ConvertRelativePositionToWorld(Vector2 relativePosition)
        {
            var screenCenter = 0f;
            
            var screenHeight = _camera.orthographicSize * 2;
            var halfScreenHeight = screenHeight / 2f;

            var screenWidth = screenHeight * _camera.aspect;
            var halfScreenWidth = screenWidth / 2f;
            
            var positionX = (screenCenter - halfScreenWidth) + (screenWidth * relativePosition.x);
            var positionY = (screenCenter - halfScreenHeight) + (screenHeight * relativePosition.y);

            return new Vector2(positionX, positionY);
        }
    }
}