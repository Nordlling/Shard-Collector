using Scellecs.Morpeh;
namespace App.Scripts.Modules.EcsWorld.Common.Extensions
{
	public static class FilterExtension
	{
		public static bool TryGetFirstEntity(this Filter filter, out Entity entity)
		{
			return (entity = filter.FirstOrDefault()) != null;
		}
	}
}