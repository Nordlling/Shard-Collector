using UnityEngine;

namespace _Main.Scripts.Global.Pool.Container
{
	public class MonoPoolParentContainer : MonoBehaviour
	{
		[SerializeField] private Transform poolParent;

		public Transform PoolParent => poolParent;
	}
}