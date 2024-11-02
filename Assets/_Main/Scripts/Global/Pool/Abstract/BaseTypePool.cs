using System;
using System.Collections.Generic;
using _Main.Scripts.Global.Pool.Interfaces.Container;
using _Main.Scripts.Global.Pool.Interfaces.Data;
using _Main.Scripts.Global.Pool.Interfaces.Items;
using _Main.Scripts.Global.Pool.Interfaces.Pool;
using Zenject;

namespace _Main.Scripts.Global.Pool.Abstract
{
	public class BaseTypePool<TPoolData, TItem> : ITypePool<TItem> where TItem : class, IPoolItem where TPoolData : IPoolData
	{
		private readonly Dictionary<Type, IMemoryPool<TItem>> _memoryPoolsMap = new Dictionary<Type, IMemoryPool<TItem>>();

		protected BaseTypePool(IFactory<TPoolData, IMemoryPool<TItem>> factory, IPoolDataProvider<Type, TPoolData> poolDataProvider)
		{
			foreach (var poolDataContainer in poolDataProvider.GetPoolData())
			{
				if (poolDataContainer == null || poolDataContainer.Key == null || poolDataContainer.PoolData == null)
				{
					throw new ArgumentException(string.Format("[BaseKeyPool] Invalid pool data of item type \"{0}\"!", typeof(TItem).Name));
				}

				if (_memoryPoolsMap.ContainsKey(poolDataContainer.Key))
				{
					throw new ArgumentException(string.Format("[BaseKeyPool] Duplicate key \"{0}\" in pool data of item type \"{1}\"!",
						poolDataContainer.Key, typeof(TItem).Name));
				}

				_memoryPoolsMap.Add(poolDataContainer.Key, factory.Create(poolDataContainer.PoolData));
			}
		}

		public T Get<T>() where T : class, TItem
		{
			var type = typeof(T);
			return GetPool(type).Spawn() as T;
		}

		public void Return(object item)
		{
			var type = item.GetType();
			GetPool(type).Despawn(item);
		}

		public void Clear()
		{
			foreach (var memoryPool in _memoryPoolsMap.Values)
			{
				memoryPool.Clear();
			}

			_memoryPoolsMap.Clear();
		}

		private IMemoryPool<TItem> GetPool(Type type)
		{
			if (_memoryPoolsMap.TryGetValue(type, out var result))
			{
				return result;
			}

			throw new KeyNotFoundException(string.Format("[BaseTypesPool] Pool doesn't contains items with type '{0}'", type));
		}
	}
}