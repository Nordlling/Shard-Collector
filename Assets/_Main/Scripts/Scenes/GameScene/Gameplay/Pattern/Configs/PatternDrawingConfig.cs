using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameScene/PatternDrawing", fileName = "PatternDrawingConfig")]
    public class PatternDrawingConfig : ScriptableObject
    {
        public bool Enabled;
        public float Threshold = 1.5f;
    }
}