using System;
using _Main.Scripts.Global.Pool.Interfaces.Data;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace _Main.Scripts.Global.DialogSystem.Configs
{
    [CreateAssetMenu(menuName = "Configs/Global/Pool/DialogsPoolPrefabs", fileName = "DialogsPoolPrefabsConfig")]
    public class DialogsPoolPrefabsConfig : SerializedScriptableObject
    {
        [OdinSerialize] [NonSerialized] public MonoTypesPoolDataProvider dialogsPoolData;
    }
}