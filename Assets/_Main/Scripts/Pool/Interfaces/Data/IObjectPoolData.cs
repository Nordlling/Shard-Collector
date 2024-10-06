using System;
namespace App.Scripts.Modules.Pool.Interfaces.Data
{
	public interface IObjectPoolData : IPoolData
	{
		Type ObjectType { get; }
	}
}