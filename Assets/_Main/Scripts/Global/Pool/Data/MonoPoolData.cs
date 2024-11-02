using System;
using _Main.Scripts.Global.Pool.Interfaces.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Main.Scripts.Global.Pool.Data
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