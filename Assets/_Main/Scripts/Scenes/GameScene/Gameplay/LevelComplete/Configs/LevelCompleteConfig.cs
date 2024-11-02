using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.LevelComplete.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameScene/LevelComplete", fileName = "LevelCompleteConfig")]
    public class LevelCompleteConfig : ScriptableObject
    {
        public int FirstStarFullPercent = 80;
        public int SecondStarFullPercent = 90;
        public int ThirdStarFullPercent = 100;
    }
}