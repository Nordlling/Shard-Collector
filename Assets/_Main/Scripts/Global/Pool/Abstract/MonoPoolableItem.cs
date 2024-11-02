using _Main.Scripts.Global.Pool.Interfaces.Items;
using Sirenix.OdinInspector;
using Zenject;

namespace _Main.Scripts.Global.Pool.Abstract
{
	public abstract class MonoPoolableItem : SerializedMonoBehaviour, IPoolItem
	{
		private IMemoryPool _pool;

		public void Remove()
		{
			_pool.Despawn(this);
		}
		
		public void OnCreateItem(IMemoryPool pool)
		{
			_pool = pool;
		}

		public void OnDestroyItem()
		{
			_pool = null;
		}
		
		public virtual void OnInitializeItem() { }

		public virtual void OnSetupItem() { }

		public virtual void OnResetItem() { }
	}
}