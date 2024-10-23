using UnityEngine;

namespace _Main.Scripts.GameScene
{
    [CreateAssetMenu(menuName = "Configs/LevelComplete", fileName = "LevelCompleteConfig")]
    public class LevelCompleteConfig : ScriptableObject
    {
        public int FirstStarFullPercent = 80;
        public int SecondStarFullPercent = 90;
        public int ThirdStarFullPercent = 100;
    }
}