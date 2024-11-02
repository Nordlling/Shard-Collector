using System.Collections.Generic;
using _Main.Scripts.Global.Pool.Interfaces.Data;

namespace _Main.Scripts.Global.Pool.Interfaces.Container
{
    public interface IPoolDataProvider<out TKey, out TPoolData> where TPoolData : IPoolData
    {
        IEnumerable<IPoolDataContainer<TKey, TPoolData>> GetPoolData();
    }
}