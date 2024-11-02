using _Main.Scripts.Global.Pool.Abstract;
using _Main.Scripts.Global.Pool.Container;
using _Main.Scripts.Global.Pool.Factory;
using _Main.Scripts.Global.Pool.Interfaces.Data;
using _Main.Scripts.Global.Pool.Interfaces.Items;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Global.Pool.Pools
{
	public class MonoPool<TItem> : BasePool<TItem, IMonoPoolData> where TItem : Component, IPoolItem
	{
		public MonoPool(DiContainer container, IMonoPoolData poolData, MonoPoolParentContainer poolParentContainer) 
			: base(new ZenjectMonoPoolFactory<TItem>(container, new MonoItemActivator<TItem>(poolParentContainer.PoolParent)), poolData) { }
	}
}