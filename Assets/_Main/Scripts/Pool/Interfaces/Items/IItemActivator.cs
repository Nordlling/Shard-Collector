namespace App.Scripts.Modules.Pool.Interfaces.Items
{
	public interface IItemActivator<in TItem> where TItem : IPoolItem
	{
		void OnCreateItem(TItem item);
		void OnSpawnItem(TItem item);
		void OnDespawnItem(TItem item);
	}
}