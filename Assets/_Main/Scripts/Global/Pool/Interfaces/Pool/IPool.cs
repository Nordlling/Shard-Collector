using _Main.Scripts.Global.Pool.Interfaces.Items;

namespace _Main.Scripts.Global.Pool.Interfaces.Pool
{
	public interface IPool
	{
		void Clear();
		void Return(object item);
	}
	
	public interface IPool<out TItem> : IPool where TItem : IPoolItem
	{
		TItem Get();
	}
}