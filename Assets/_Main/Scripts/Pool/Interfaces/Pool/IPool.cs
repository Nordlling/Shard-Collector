using App.Scripts.Modules.Pool.Interfaces.Items;
namespace App.Scripts.Modules.Pool.Interfaces.Pool
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