using System;
using App.Scripts.Modules.Pool.Interfaces.Data;
using Sirenix.OdinInspector;
using UnityEngine;
namespace App.Scripts.Modules.Pool.Data
{
	[Serializable]
	public class MonoPoolData : IMonoPoolData
	{
		public int initialSize;
		[AssetsOnly]
		public GameObject prefab;

		public int InitialSize => initialSize;
		public GameObject Prefab => prefab;
	}
}