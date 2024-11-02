using _Main.Scripts.Global.Pool.Interfaces.Data;
using _Main.Scripts.Global.Pool.Interfaces.Items;
using _Main.Scripts.Global.Pool.Interfaces.Pool;
using Zenject;

namespace _Main.Scripts.Global.Pool.Abstract
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