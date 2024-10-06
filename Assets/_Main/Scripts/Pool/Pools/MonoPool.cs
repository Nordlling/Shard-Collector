using App.Scripts.Modules.Pool.Abstract;
using App.Scripts.Modules.Pool.Container;
using App.Scripts.Modules.Pool.Factory;
using App.Scripts.Modules.Pool.Interfaces.Data;
using App.Scripts.Modules.Pool.Interfaces.Items;
using UnityEngine;
using Zenject;

namespace App.Scripts.Modules.Pool.Pools
{
	public class MonoPool<TItem> : BasePool<TItem, IMonoPoolData> where TItem : Component, IPoolItem
	{
		public MonoPool(DiContainer container, IMonoPoolData poolData, MonoPoolParentContainer poolParentContainer) 
			: base(new ZenjectMonoPoolFactory<TItem>(container, new MonoItemActivator<TItem>(poolParentContainer.PoolParent)), poolData) { }
	}
}