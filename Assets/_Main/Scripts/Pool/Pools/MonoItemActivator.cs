using App.Scripts.Modules.Pool.Interfaces.Items;
using UnityEngine;
namespace App.Scripts.Modules.Pool.Pools
{
	public class MonoItemActivator<TItem> : IItemActivator<TItem> where TItem : Component, IPoolItem
	{
		private readonly Transform _parentTransform;

		public MonoItemActivator(Transform parentTransform = null)
		{
			_parentTransform = CreateDefaultParentTransform();

			if (parentTransform != null)
			{
				_parentTransform.SetParent(parentTransform);
			}
		}

		private Transform CreateDefaultParentTransform()
		{
			var gameObject = new GameObject($"{typeof(TItem).Name}Pool");
			return gameObject.transform;
		}

		public void OnCreateItem(TItem item)
		{
			DeactivateItem(item);
		}

		public void OnSpawnItem(TItem item)
		{
			ActivateItem(item);
		}

		public void OnDespawnItem(TItem item)
		{
			DeactivateItem(item);
		}

		private void ActivateItem(TItem item)
		{
			item.gameObject.SetActive(true);
		}

		private void DeactivateItem(TItem item)
		{
			item.gameObject.SetActive(false);
			item.transform.SetParent(_parentTransform, false);
		}
	}
}