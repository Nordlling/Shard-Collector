using UnityEngine;

namespace App.Scripts.Scenes.Game.Configs.Pool
{
    [CreateAssetMenu(menuName = "Configs/GameScene/LevelConfig", fileName = "LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public string DefaultLevelPath;
    }
}