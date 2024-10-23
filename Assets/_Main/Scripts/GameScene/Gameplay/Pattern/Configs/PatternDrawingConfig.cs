using UnityEngine;

namespace _Main.Scripts.Pattern
{
    [CreateAssetMenu(menuName = "Configs/PatternDrawing", fileName = "PatternDrawingConfig")]
    public class PatternDrawingConfig : ScriptableObject
    {
        public float Threshold = 1.5f;
    }
}