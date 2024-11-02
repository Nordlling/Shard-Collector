using System;
using _Main.Scripts.Global.Pool.Abstract;
using _Main.Scripts.Global.Pool.Container;
using _Main.Scripts.Global.Pool.Factory;
using _Main.Scripts.Global.Pool.Interfaces.Container;
using _Main.Scripts.Global.Pool.Interfaces.Data;
using _Main.Scripts.Global.Pool.Interfaces.Items;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Global.Pool.Pools
{
	public class MonoTypePool<TItem> : BaseTypePool<IMonoPoolData, TItem> where TItem : Component, IPoolItem
	{
		public MonoTypePool(DiContainer container, IPoolDataProvider<Type, IMonoPoolData> poolDataProvider, MonoPoolParentContainer poolParentContainer) 
			: base(new ZenjectMonoPoolFactory<TItem>(container, new MonoItemActivator<TItem>(poolParentContainer.PoolParent)), poolDataProvider) { }
	}
}