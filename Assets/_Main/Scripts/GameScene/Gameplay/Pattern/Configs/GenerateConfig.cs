using UnityEngine;

namespace _Main.Scripts.Pattern
{
    [CreateAssetMenu(menuName = "Configs/Generate", fileName = "GenerateConfig")]
    public class GenerateConfig : ScriptableObject
    {
        public float Angle = 20f;
        public int MinShapesCount = 4;
        public int MaxShapesCount = 11;
    }
}