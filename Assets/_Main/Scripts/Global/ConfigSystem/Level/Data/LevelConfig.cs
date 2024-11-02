using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Main.Scripts.Global.ConfigSystem.Level.Data
{
    [CreateAssetMenu(menuName = "Configs/Global/LevelConfig", fileName = "LevelConfig")]
    public class LevelConfig : SerializedScriptableObject
    {
        public bool UseDefaultLevel;
        public string DefaultLevelPath;
        public int FirstLevelIndex;
        public Dictionary<int, string> LevelsMap;
    }
}