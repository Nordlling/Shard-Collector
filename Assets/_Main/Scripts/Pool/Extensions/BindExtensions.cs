using App.Scripts.Modules.Pool.Container;
using App.Scripts.Modules.Pool.Data;
using App.Scripts.Modules.Pool.Interfaces.Items;
using App.Scripts.Modules.Pool.Interfaces.Pool;
using App.Scripts.Modules.Pool.Pools;
using UnityEngine;
using Zenject;
namespace App.Scripts.Modules.Pool.Extensions
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
	}
}