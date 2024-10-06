using App.Scripts.Modules.Pool.Interfaces.Data;
using App.Scripts.Modules.Pool.Interfaces.Items;
using App.Scripts.Modules.Pool.MemoryPool;
using Zenject;
namespace App.Scripts.Modules.Pool.Factory
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