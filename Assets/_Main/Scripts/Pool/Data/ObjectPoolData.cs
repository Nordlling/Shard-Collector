using System;
using App.Scripts.Modules.Pool.Interfaces.Data;
namespace App.Scripts.Modules.Pool.Data
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