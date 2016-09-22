using System.Collections.Generic;

namespace AlgoKit.Extensions
{
    internal static class ObjectExtensions
    {
        /// <summary>
        /// Wraps this object instance into an <see cref="IEnumerable{T}"/>
        /// consisting of a single item.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="item">The instance that will be wrapped.</param>
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }
    }
}
