using System;
using App.Scripts.Modules.Pool.Interfaces.Container;
using App.Scripts.Modules.Pool.Interfaces.Data;
namespace App.Scripts.Modules.Pool.Data
{
	[Serializable]
	public class MonoTypesPoolDataContainer : IPoolDataContainer<Type, IMonoPoolData>
	{
		public MonoTypesPoolData poolData;
		public Type Key => poolData.type;
		public IMonoPoolData PoolData => poolData;
	}
}