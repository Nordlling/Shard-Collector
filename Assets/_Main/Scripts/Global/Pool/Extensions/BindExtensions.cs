using System;
using _Main.Scripts.Global.Pool.Container;
using _Main.Scripts.Global.Pool.Data;
using _Main.Scripts.Global.Pool.Interfaces.Container;
using _Main.Scripts.Global.Pool.Interfaces.Data;
using _Main.Scripts.Global.Pool.Interfaces.Items;
using _Main.Scripts.Global.Pool.Interfaces.Pool;
using _Main.Scripts.Global.Pool.Pools;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Global.Pool.Extensions
{
	public static class BindExtensions
	{
		public static InstantiateCallbackConditionCopyNonLazyBinder BindObjectsPool<TItem>(this DiContainer container, ObjectPoolData poolData)
			where TItem : class, IPoolItem
		{
			return BindObjectsPool<TItem, TItem>(container, poolData);
		}

		public static InstantiateCallbackConditionCopyNonLazyBinder BindObjectsPool<TContractItem, TItem>(this DiContainer container, ObjectPoolData poolData)
			where TContractItem : IPoolItem where TItem : class, TContractItem
		{
			return container.Bind<IPool<TContractItem>>().To<ObjectsPool<TItem>>().AsSingle()
				.WithArguments(poolData);
		}

		public static InstantiateCallbackConditionCopyNonLazyBinder BindMonoPool<TItem>(this DiContainer container, MonoPoolData poolData, MonoPoolParentContainer poolParentContainer)
			where TItem : Component, IPoolItem
		{
			return BindMonoPool<TItem, TItem>(container, poolData, poolParentContainer);
		}

		public static InstantiateCallbackConditionCopyNonLazyBinder BindMonoPool<TContractItem, TItem>(this DiContainer container, MonoPoolData poolData, MonoPoolParentContainer poolParentContainer)
			where TContractItem : IPoolItem where TItem : Component, TContractItem
		{
			return container.Bind<IPool<TContractItem>>().To<MonoPool<TItem>>().AsSingle()
				.WithArguments(poolData, poolParentContainer);
		}
		
		public static InstantiateCallbackConditionCopyNonLazyBinder BindMonoTypesPool<TItem>(this DiContainer container, IPoolDataProvider<Type, IMonoPoolData> poolDataProvider, MonoPoolParentContainer poolParentContainer)
			where TItem : Component, IPoolItem
		{
			return container.Bind<ITypePool<TItem>>().To<MonoTypePool<TItem>>().AsSingle()
				.WithArguments(poolDataProvider, poolParentContainer);
		}
	}
}