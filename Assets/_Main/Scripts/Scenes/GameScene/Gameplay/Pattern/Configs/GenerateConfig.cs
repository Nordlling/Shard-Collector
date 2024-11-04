using System;
using _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameScene/Generate", fileName = "GenerateConfig")]
    public class GenerateConfig : ScriptableObject
    {
        [OnValueChanged(nameof(OnValueChanged))] public ShapesGenerateInfo ShapesGenerateInfo;
        
        private void OnValueChanged()
        {
            if (ShapesGenerateInfo.MinShapeAreaFraction > ShapesGenerateInfo.MaxShapeAreaFraction)
            {
                ShapesGenerateInfo.MinShapeAreaFraction = ShapesGenerateInfo.MaxShapeAreaFraction;
            }

            ShapesGenerateInfo.MinShapeAreaFraction = (float) Math.Round(ShapesGenerateInfo.MinShapeAreaFraction, 2);
            ShapesGenerateInfo.MaxShapeAreaFraction = (float) Math.Round(ShapesGenerateInfo.MaxShapeAreaFraction, 2);
        }
    }
}