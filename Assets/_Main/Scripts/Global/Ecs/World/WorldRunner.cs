using Scellecs.Morpeh;

namespace _Main.Scripts.Global.Ecs.World
{
	public class WorldRunner : IWorldRunner
	{
		private int _systemOrder;
		private Scellecs.Morpeh.World _world;

		public Scellecs.Morpeh.World World => _world;

		public WorldRunner()
		{
		}

		public Entity CreateEntity()
		{
			return _world.CreateEntity();
		}

		public void CreateWorld(ISystemGroupContainer groupContainer)
		{
			_world = Scellecs.Morpeh.World.Create();

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