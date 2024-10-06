using App.Scripts.Modules.Pool.Interfaces.Items;
namespace App.Scripts.Modules.Pool.Interfaces.Pool
{
	public interface IKeyPool
	{
		void Clear();
		void Return(object key, object item);
	}

	public interface IKeyPool<in TKey, out TItem> : IKeyPool where TItem : IPoolItem
	{
		TItem Get(TKey key);
	}
}