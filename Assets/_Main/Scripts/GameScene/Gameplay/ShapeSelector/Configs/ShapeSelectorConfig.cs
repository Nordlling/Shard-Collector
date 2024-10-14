using UnityEngine;

namespace _Main.Scripts { 
    
    [CreateAssetMenu(menuName = "Configs/GameScene/ShapeSelectorConfig", fileName = "ShapeSelectorConfig")]
    public class ShapeSelectorConfig : ScriptableObject
    {
        [Range(0f, 1f)] public float RelativePositionX;
        [Range(0f, 1f)] public float RelativePositionY;
        [Range(0f, 1f)] public float RelativeWidth;
        [Range(0f, 1f)] public float RelativeHeight;
        [Range(0f, 1f)] public float RelativePaddingTop;
        [Range(0f, 1f)] public float RelativePaddingBottom;
        [Range(0f, 1f)] public float CellOffset;
        [Min(0)] public int MaxCount;
    }
}