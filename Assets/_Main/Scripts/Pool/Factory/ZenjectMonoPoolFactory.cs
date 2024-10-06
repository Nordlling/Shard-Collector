using App.Scripts.Modules.Pool.Interfaces.Data;
using App.Scripts.Modules.Pool.Interfaces.Items;
using App.Scripts.Modules.Pool.MemoryPool;
using UnityEngine;
using Zenject;
namespace App.Scripts.Modules.Pool.Factory
{
	internal class ZenjectMonoPoolFactory<TItem> : IFactory<IMonoPoolData, IMemoryPool<TItem>> where TItem : Component, IPoolItem
	{
		private readonly DiContainer _container;
		private readonly IItemActivator<TItem> _itemActivator;

		internal ZenjectMonoPoolFactory(DiContainer container, IItemActivator<TItem> itemActivator)
		{
			_container = container;
			_itemActivator = itemActivator;
		}

		public IMemoryPool<TItem> Create(IMonoPoolData poolData)
		{
			var settings = new MemoryPoolSettings(poolData.InitialSize, int.MaxValue, PoolExpandMethods.OneAtATime);

			var pool = _container.Instantiate<ZenjectMemoryPool<TItem>>(
				new object[] { _itemActivator, settings, new ZenjectMonoFactory<TItem>(_container, poolData) });

			return pool;
		}
	}
}