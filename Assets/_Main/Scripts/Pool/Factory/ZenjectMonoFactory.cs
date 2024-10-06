using App.Scripts.Modules.Pool.Interfaces.Data;
using App.Scripts.Modules.Pool.Interfaces.Items;
using UnityEngine;
using Zenject;

namespace App.Scripts.Modules.Pool.Factory
{
	internal class ZenjectMonoFactory<TItem> : IFactory<TItem> where TItem : Component, IPoolItem
	{
		private readonly DiContainer _container;
		private readonly IMonoPoolData _poolData;

		internal ZenjectMonoFactory(DiContainer container, IMonoPoolData poolData)
		{
			_container = container;
			_poolData = poolData;
		}
		
		public TItem Create()
		{
			return _container.InstantiatePrefabForComponent<TItem>(_poolData.Prefab);
		}
	}
}