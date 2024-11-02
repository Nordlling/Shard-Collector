using Scellecs.Morpeh;

namespace _Main.Scripts.Global.Ecs.World
{
	public interface ISystemGroupContainer
	{
		void SetupSystems(SystemsGroup systemsGroup);
	}
}