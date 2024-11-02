using _Main.Scripts.Global.Pool.Abstract;
using _Main.Scripts.Global.Pool.Factory;
using _Main.Scripts.Global.Pool.Interfaces.Data;
using _Main.Scripts.Global.Pool.Interfaces.Items;
using Zenject;

namespace _Main.Scripts.Global.Pool.Pools
{
	public class ObjectsPool<TItem> : BasePool<TItem, IObjectPoolData> where TItem : class, IPoolItem
	{
		public ObjectsPool(DiContainer container, IObjectPoolData poolData) : 
			base(new ZenjectObjectsPoolFactory<TItem>(container), poolData) { }
	}
}