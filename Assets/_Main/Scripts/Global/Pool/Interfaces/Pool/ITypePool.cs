using _Main.Scripts.Global.Pool.Interfaces.Items;

namespace _Main.Scripts.Global.Pool.Interfaces.Pool
{
	public interface ITypePool
	{
		void Clear();
		void Return(object item);
	}

	public interface ITypePool<TItem> : ITypePool where TItem : IPoolItem
	{
		T Get<T>() where T : class, TItem;
	}
}