using System;
using App.Scripts.Modules.Pool.Abstract;
using App.Scripts.Modules.Pool.Container;
using App.Scripts.Modules.Pool.Factory;
using App.Scripts.Modules.Pool.Interfaces.Container;
using App.Scripts.Modules.Pool.Interfaces.Data;
using App.Scripts.Modules.Pool.Interfaces.Items;
using UnityEngine;
using Zenject;
namespace App.Scripts.Modules.Pool.Pools
{
	public class MonoTypePool<TItem> : BaseTypePool<IMonoPoolData, TItem> where TItem : Component, IPoolItem
	{
		public MonoTypePool(DiContainer container, IPoolDataProvider<Type, IMonoPoolData> poolDataProvider, MonoPoolParentContainer poolParentContainer) 
			: base(new ZenjectMonoPoolFactory<TItem>(container, new MonoItemActivator<TItem>(poolParentContainer.PoolParent)), poolDataProvider) { }
	}
}