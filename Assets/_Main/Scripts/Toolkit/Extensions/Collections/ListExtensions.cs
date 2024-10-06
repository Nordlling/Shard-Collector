using System.Collections;
using App.Scripts.Modules.Utils.RandomService;

namespace _Main.Scripts 
{
    public static class ListExtensions
    {
        public static void ShuffleRandom(this IList list, IRandomService randomService)
        {
            for (var i = list.Count - 1; i >= 1; i--)
            {
                var j = randomService.Range(0, i + 1);
                (list[j], list[i]) = (list[i], list[j]);
            }
        }
        
    }
}