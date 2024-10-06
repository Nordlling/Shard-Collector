using App.Scripts.Modules.EcsWorld.Infrastructure.Systems;
using Scellecs.Morpeh;
namespace App.Scripts.Modules.EcsWorld.Infrastructure.Services
{
	public interface IWorldRunner
	{
		World World { get; }
		Entity CreateEntity();
		void CreateWorld(ISystemGroupContainer groupContainer);
		void Update(float deltaTime);
		void DestroyWorld();
	}
}