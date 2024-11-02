using UnityEngine;

namespace _Main.Scripts.Global.Pool.Interfaces.Data
{
	public interface IMonoPoolData : IPoolData
	{
		GameObject Prefab { get; }
	}
}