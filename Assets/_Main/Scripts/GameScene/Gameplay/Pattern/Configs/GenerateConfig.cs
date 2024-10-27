using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Main.Scripts.Pattern
{
    [CreateAssetMenu(menuName = "Configs/Generate", fileName = "GenerateConfig")]
    public class GenerateConfig : ScriptableObject
    {
        public float Angle = 20f;
        [Range(0f, 1f)] [OnValueChanged(nameof(OnValueChanged))] public float MinShapeAreaFraction = 0.05f;
        [Range(0f, 1f)] [OnValueChanged(nameof(OnValueChanged))] public float MaxShapeAreaFraction = 0.15f;
        
        
        private void OnValueChanged()
        {
            if (MinShapeAreaFraction > MaxShapeAreaFraction)
            {
                MinShapeAreaFraction = MaxShapeAreaFraction;
            }

            MinShapeAreaFraction = (float) Math.Round(MinShapeAreaFraction, 2);
            MaxShapeAreaFraction = (float) Math.Round(MaxShapeAreaFraction, 2);
        }
    }
}