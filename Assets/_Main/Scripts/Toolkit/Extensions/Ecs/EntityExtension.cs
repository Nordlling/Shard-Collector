using Scellecs.Morpeh;
namespace App.Scripts.Modules.EcsWorld.Common.Extensions
{
	public static class EntityExtension
	{
		public static void TryAddComponent<T>(this Entity entity) where T : struct, IComponent
		{
			if (!entity.Has<T>())
			{
				entity.AddComponent<T>();
			}
		}
	}
}