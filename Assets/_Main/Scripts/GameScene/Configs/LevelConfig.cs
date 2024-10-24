using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.Game.Configs.Pool
{
    [CreateAssetMenu(menuName = "Configs/GameScene/LevelConfig", fileName = "LevelConfig")]
    public class LevelConfig : SerializedScriptableObject
    {
        public bool UseDefaultLevel;
        public string DefaultLevelPath;
        public int FirstLevelIndex;
        public Dictionary<int, string> LevelsMap;
    }
}