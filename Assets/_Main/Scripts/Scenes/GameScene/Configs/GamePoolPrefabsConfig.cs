using System;
using _Main.Scripts.Global.Pool.Data;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameScene/Pool/GamePoolPrefabsConfig", fileName = "GamePoolPrefabsConfig")]
    public class GamePoolPrefabsConfig : SerializedScriptableObject
    {
        [OdinSerialize] [NonSerialized] public MonoPoolData shapePoolData;
    }
}