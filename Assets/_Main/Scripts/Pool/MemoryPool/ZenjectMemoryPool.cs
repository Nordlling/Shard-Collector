using App.Scripts.Modules.Pool.Interfaces.Items;
using Zenject;
namespace App.Scripts.Modules.Pool.MemoryPool
{
	internal class ZenjectMemoryPool<TItem> : MemoryPool<TItem> where TItem : IPoolItem
	{
		private readonly IItemActivator<TItem> _itemActivator;

		public ZenjectMemoryPool(IItemActivator<TItem> itemActivator = null)
		{
			_itemActivator = itemActivator;
		}

		protected override void OnCreated(TItem item)
		{
			_itemActivator?.OnCreateItem(item);
			item.OnCreateItem(this);
			item.OnInitializeItem();
		}

		protected override void OnSpawned(TItem item)
		{
			item.OnSetupItem();
			_itemActivator?.OnSpawnItem(item);
		}

		protected override void OnDespawned(TItem item)
		{
			_itemActivator?.OnDespawnItem(item);
			item.OnResetItem();
		}

		protected override void OnDestroyed(TItem item)
		{
			item.OnDestroyItem();
		}

	}
}