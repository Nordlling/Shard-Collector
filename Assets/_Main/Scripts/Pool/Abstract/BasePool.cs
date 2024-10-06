using App.Scripts.Modules.Pool.Interfaces.Data;
using App.Scripts.Modules.Pool.Interfaces.Items;
using App.Scripts.Modules.Pool.Interfaces.Pool;
using Zenject;
namespace App.Scripts.Modules.Pool.Abstract
{
	public abstract class BasePool<TItem, TPoolData> : IPool<TItem> where TItem : class, IPoolItem where TPoolData : IPoolData
	{
		private readonly IMemoryPool<TItem> _memoryPool;

		protected BasePool(IFactory<TPoolData, IMemoryPool<TItem>> factory, TPoolData poolData)
		{
			_memoryPool = factory.Create(poolData);
		}

		public TItem Get()
		{
			return _memoryPool.Spawn();
		}

		public void Return(object item)
		{
			_memoryPool.Despawn(item);
		}

		public void Clear()
		{
			_memoryPool.Clear();
		}

	}
}