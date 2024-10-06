using UnityEngine;
namespace App.Scripts.Modules.Pool.Interfaces.Data
{
	public interface IMonoPoolData : IPoolData
	{
		GameObject Prefab { get; }
	}
}