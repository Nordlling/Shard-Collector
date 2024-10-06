using UnityEngine;
namespace App.Scripts.Modules.Pool.Container
{
	public class MonoPoolParentContainer : MonoBehaviour
	{
		[SerializeField] private Transform poolParent;

		public Transform PoolParent => poolParent;
	}
}