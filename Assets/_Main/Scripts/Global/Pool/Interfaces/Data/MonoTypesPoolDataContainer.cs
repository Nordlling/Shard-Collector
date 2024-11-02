using System;
using _Main.Scripts.Global.Pool.Interfaces.Container;

namespace _Main.Scripts.Global.Pool.Interfaces.Data
{
	[Serializable]
	public class MonoTypesPoolDataContainer : IPoolDataContainer<Type, IMonoPoolData>
	{
		public MonoTypesPoolData poolData;
		public Type Key => poolData.type;
		public IMonoPoolData PoolData => poolData;
	}
}