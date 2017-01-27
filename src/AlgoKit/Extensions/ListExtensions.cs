using System.Collections.Generic;

namespace AlgoKit.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Swaps two elements of a collection, given their indexes.
        /// </summary>
        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            var tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }
}