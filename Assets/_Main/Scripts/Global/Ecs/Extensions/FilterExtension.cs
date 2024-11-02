using Scellecs.Morpeh;

namespace _Main.Scripts.Global.Ecs.Extensions
{
	public static class FilterExtension
	{
		public static bool TryGetFirstEntity(this Filter filter, out Entity entity)
		{
			return (entity = filter.FirstOrDefault()) != null;
		}
	}
}