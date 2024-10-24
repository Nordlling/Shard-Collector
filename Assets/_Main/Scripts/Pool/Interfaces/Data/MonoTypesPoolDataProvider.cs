using System;
using System.Collections.Generic;
using App.Scripts.Modules.Pool.Interfaces.Container;
using App.Scripts.Modules.Pool.Interfaces.Data;
using UnityEngine;
namespace App.Scripts.Modules.Pool.Data
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