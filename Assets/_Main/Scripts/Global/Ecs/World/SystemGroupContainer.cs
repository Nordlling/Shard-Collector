using System.Collections.Generic;
using Scellecs.Morpeh;

namespace _Main.Scripts.Global.Ecs.World
{
	public sealed class SystemGroupContainer : ISystemGroupContainer
	{
		private readonly IEnumerable<IInitializer> _initializers;
		private readonly IEnumerable<ISystem> _systems;

		public SystemGroupContainer(List<IInitializer> initializers, List<ISystem> systems)
		{
			_initializers = initializers;
			_systems = systems;
		}

		public void SetupSystems(SystemsGroup systemsGroup)
		{
			foreach (var initializer in _initializers)
			{
				systemsGroup.AddInitializer(initializer);
			}

			foreach (var system in _systems)
			{
				systemsGroup.AddSystem(system);
			}
		}
	}
}