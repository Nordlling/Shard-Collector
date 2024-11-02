using _Main.Scripts.Global.Pool.Interfaces.Data;
using _Main.Scripts.Global.Pool.Interfaces.Items;
using Zenject;

namespace _Main.Scripts.Global.Pool.Factory
{
	public class ZenjectObjectFactory<TItem> : IFactory<TItem> where TItem : class, IPoolItem
	{
		private readonly DiContainer _container;
		private readonly IObjectPoolData _poolData;

		internal ZenjectObjectFactory(DiContainer container, IObjectPoolData poolData)
		{
			_poolData = poolData;
			_container = container;
		}

		public TItem Create()
		{
			return (TItem) _container.Instantiate(_poolData.ObjectType);
		}
	}
}