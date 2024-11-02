using System;
using System.Collections.Generic;
using _Main.Scripts.Global.Pool.Interfaces.Container;
using UnityEngine;

namespace _Main.Scripts.Global.Pool.Interfaces.Data
{
	[Serializable]
	public class MonoTypesPoolDataProvider : IPoolDataProvider<Type, IMonoPoolData>
	{
		[SerializeField] private List<MonoTypesPoolDataContainer> dataContainers = new List<MonoTypesPoolDataContainer>();
		
		public IEnumerable<IPoolDataContainer<Type, IMonoPoolData>> GetPoolData()
		{
			return dataContainers;
		}
	}
}