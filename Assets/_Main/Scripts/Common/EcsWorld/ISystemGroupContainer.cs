using Scellecs.Morpeh;
namespace App.Scripts.Modules.EcsWorld.Infrastructure.Systems
{
	public interface ISystemGroupContainer
	{
		void SetupSystems(SystemsGroup systemsGroup);
	}
}