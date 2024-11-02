using System;

namespace _Main.Scripts.Global.Pool.Interfaces.Data
{
	public interface IObjectPoolData : IPoolData
	{
		Type ObjectType { get; }
	}
}