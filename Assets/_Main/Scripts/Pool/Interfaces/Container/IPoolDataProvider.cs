using System.Collections.Generic;
using App.Scripts.Modules.Pool.Interfaces.Data;
namespace App.Scripts.Modules.Pool.Interfaces.Container
{
    public interface IPoolDataProvider<out TKey, out TPoolData> where TPoolData : IPoolData
    {
        IEnumerable<IPoolDataContainer<TKey, TPoolData>> GetPoolData();
    }
}