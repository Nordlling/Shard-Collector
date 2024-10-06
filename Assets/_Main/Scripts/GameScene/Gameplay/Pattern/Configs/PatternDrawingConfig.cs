using UnityEngine;

namespace _Main.Scripts.Pattern
{
    [CreateAssetMenu(menuName = "Configs/PatternDrawing", fileName = "PatternDrawingConfig")]
    public class PatternDrawingConfig : ScriptableObject
    {
        public float Angle = 20f;
        public float Threshold = 1.5f;
        public bool Divide;
    }
}