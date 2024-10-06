using App.Scripts.Modules.Pool.Interfaces.Data;
namespace App.Scripts.Modules.Pool.Interfaces.Container
{
	public interface IPoolDataContainer<out TKey, out TPoolData> where TPoolData : IPoolData
	{
		TKey Key { get; }
		TPoolData PoolData { get; }
	}
}