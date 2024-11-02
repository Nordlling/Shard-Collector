using System;
using _Main.Scripts.Global.Pool.Interfaces.Data;

namespace _Main.Scripts.Global.Pool.Data
{
	public class ObjectPoolData : IObjectPoolData
	{
		public ObjectPoolData(Type objectType, int initialSize = 0)
		{
			ObjectType = objectType;
			InitialSize = initialSize;
		}
		
		public int InitialSize { get; }
		public Type ObjectType { get; }
	}
}