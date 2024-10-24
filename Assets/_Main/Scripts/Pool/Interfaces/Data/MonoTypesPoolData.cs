using System;
using App.Scripts.Modules.Pool.Abstract;
using App.Scripts.Modules.Pool.Interfaces.Data;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
namespace App.Scripts.Modules.Pool.Data
{
	[Serializable]
	public class MonoTypesPoolData : IMonoPoolData
	{
		[OdinSerialize] [ReadOnly]
		public Type type;

		public int initialSize;

		[AssetsOnly] [OnValueChanged("UpdatePrefabType")]
		public GameObject prefab;
		
		public int InitialSize => initialSize;
		public GameObject Prefab => prefab.gameObject;

#if UNITY_EDITOR

		private bool ValidateDialogPrefab()
		{
			if (prefab == null)
			{
				return false;
			}

			return true;
		}

		private void UpdatePrefabType()
		{
			type = prefab.GetComponent<MonoPoolableItem>().GetType();
		}
#endif
	}
}