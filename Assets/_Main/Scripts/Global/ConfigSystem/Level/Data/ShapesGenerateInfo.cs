using System;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Data
{
    [Serializable]
    public class ShapesGenerateInfo
    {
        [Min(0f)] public float Angle = 40f;
        [Range(0f, 1f)] public float MinShapeAreaFraction = 0.15f;
        [Range(0f, 1f)] public float MaxShapeAreaFraction = 0.6f;
    }
}