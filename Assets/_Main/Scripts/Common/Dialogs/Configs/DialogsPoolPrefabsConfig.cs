using System;
using App.Scripts.Modules.Pool.Data;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
namespace App.Scripts.Scenes.Game.Configs.Pool
{
    [CreateAssetMenu(menuName = "Configs/GameScene/Pool/DialogsPoolPrefabs", fileName = "DialogsPoolPrefabsConfig")]
    public class DialogsPoolPrefabsConfig : SerializedScriptableObject
    {
        [OdinSerialize] [NonSerialized] public MonoTypesPoolDataProvider dialogsPoolData;
    }
}