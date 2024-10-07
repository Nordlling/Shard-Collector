using System;
using App.Scripts.Modules.Pool.Data;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
namespace App.Scripts.Scenes.Game.Configs.Pool
{
    [CreateAssetMenu(menuName = "Configs/GameScene/Pool/GamePoolPrefabsConfig", fileName = "GamePoolPrefabsConfig")]
    public class GamePoolPrefabsConfig : SerializedScriptableObject
    {
        [OdinSerialize] [NonSerialized] public MonoPoolData shapePoolData;
    }
}