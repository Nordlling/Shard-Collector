using _Main.Scripts.Global.Pool.Interfaces.Data;
using _Main.Scripts.Global.Pool.Interfaces.Items;
using _Main.Scripts.Global.Pool.MemoryPool;
using Zenject;

namespace _Main.Scripts.Global.Pool.Factory
{
	public class ZenjectObjectsPoolFactory<TItem> : IFactory<IObjectPoolData, IMemoryPool<TItem>> where TItem : class, IPoolItem
	{
		private readonly DiContainer _container;

		public ZenjectObjectsPoolFactory(DiContainer container)
		{
			_container = container;
		}

		public IMemoryPool<TItem> Create(IObjectPoolData poolData)
		{
			var settings = new MemoryPoolSettings(poolData.InitialSize, int.MaxValue, PoolExpandMethods.OneAtATime);

			var pool = _container.Instantiate<ZenjectMemoryPool<TItem>>(
				new object[] {settings, new ZenjectObjectFactory<TItem>(_container, poolData)});

			return pool;
		}
	}
}