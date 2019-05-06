using System.Collections.Generic;
using System.Linq;

namespace Gybs.Extensions
{
    /// <summary>
    /// <see cref="System.Linq"/> related extensions.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Casts the <see cref="List{T}"/> type to the <see cref="IList{T}"/> type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>The <see cref="IReadOnlyCollection{T}"/>.</returns>
        public static IReadOnlyCollection<T> CastToReadOnly<T>(this List<T> list) => list;

        /// <summary>
        /// Casts the array to the <see cref="IReadOnlyList{T}"/> type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="array">The array..</param>
        /// <returns>The <see cref="IReadOnlyCollection{T}"/>.</returns>
        public static IReadOnlyCollection<T> CastToReadOnly<T>(this T[] array) => array;

        /// <summary>
        /// Flattens the enumerable of enumerables into a single enumerable.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="enumerable">The enumerables.</param>
        /// <returns>Enumerable.</returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> enumerable) => enumerable.SelectMany(e => e);
    }
}
