using App.Scripts.Modules.Pool.Interfaces.Data;
using App.Scripts.Modules.Pool.Interfaces.Items;
using Zenject;
namespace App.Scripts.Modules.Pool.Factory
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