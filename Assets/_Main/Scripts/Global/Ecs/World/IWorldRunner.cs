using Scellecs.Morpeh;

namespace _Main.Scripts.Global.Ecs.World
{
	public interface IWorldRunner
	{
		Scellecs.Morpeh.World World { get; }
		Entity CreateEntity();
		void CreateWorld(ISystemGroupContainer groupContainer);
		void Update(float deltaTime);
		void DestroyWorld();
	}
}