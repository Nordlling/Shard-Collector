using App.Scripts.Modules.EcsWorld.Infrastructure.Systems;
using Scellecs.Morpeh;
namespace App.Scripts.Modules.EcsWorld.Infrastructure.Services
{
	internal class WorldRunner : IWorldRunner
	{
		private int _systemOrder;
		private World _world;

		public World World => _world;

		public WorldRunner()
		{
		}

		public Entity CreateEntity()
		{
			return _world.CreateEntity();
		}

		public void CreateWorld(ISystemGroupContainer groupContainer)
		{
			_world = World.Create();

			var systemsGroup = _world.CreateSystemsGroup();
			_world.AddSystemsGroup(_systemOrder++, systemsGroup);

			groupContainer.SetupSystems(systemsGroup);
		}

		public void Update(float deltaTime)
		{
			_world.Update(deltaTime);
			_world.CleanupUpdate(deltaTime);
		}

		public void DestroyWorld()
		{
			if (_world != null)
			{
				_world.Dispose();
				_world = null;	
			}
		}
	}
}