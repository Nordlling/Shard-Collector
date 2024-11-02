using _Main.Scripts.Global.Pool.Interfaces.Data;

namespace _Main.Scripts.Global.Pool.Interfaces.Container
{
	public interface IPoolDataContainer<out TKey, out TPoolData> where TPoolData : IPoolData
	{
		TKey Key { get; }
		TPoolData PoolData { get; }
	}
}