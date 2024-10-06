using App.Scripts.Modules.Pool.Abstract;
using App.Scripts.Modules.Pool.Factory;
using App.Scripts.Modules.Pool.Interfaces.Data;
using App.Scripts.Modules.Pool.Interfaces.Items;
using Zenject;
namespace App.Scripts.Modules.Pool.Pools
{
	public class ObjectsPool<TItem> : BasePool<TItem, IObjectPoolData> where TItem : class, IPoolItem
	{
		public ObjectsPool(DiContainer container, IObjectPoolData poolData) : 
			base(new ZenjectObjectsPoolFactory<TItem>(container), poolData) { }
	}
}