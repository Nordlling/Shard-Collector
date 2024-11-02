using System;
using _Main.Scripts.Global.Pool.Abstract;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace _Main.Scripts.Global.Pool.Interfaces.Data
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