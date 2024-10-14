using UnityEngine;

namespace _Main.Scripts.Toolkit.Screen
{
    public interface IScreenService
    {
        Rect CalculateRect(float relativePositionX, float relativePositionY, float relativeWidth, float relativeHeight);
        Vector2 ConvertRelativeSizeToWorld(Vector2 relativeSize);
        Vector2 ConvertRelativePositionToWorld(Vector2 relativePosition);
    }
}