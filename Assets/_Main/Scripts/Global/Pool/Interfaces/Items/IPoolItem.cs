using Zenject;

namespace _Main.Scripts.Global.Pool.Interfaces.Items
{
	public interface IPoolItem
	{
		void Remove();
		void OnCreateItem(IMemoryPool pool);
		void OnInitializeItem();
		void OnSetupItem();
		void OnResetItem();
		void OnDestroyItem();
		void OnDisposeItem();
	}
}