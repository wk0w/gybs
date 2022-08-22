﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gybs.Extensions;

/// <summary>
/// <see cref="IEnumerable{T}"/> extensions.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Performs the specified action on each element of the <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="action">The action to perform.</param>
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var element in enumerable)
        {
            action(element);
        }
    }

    /// <summary>
    /// Performs the specified asynchronous action on each element of the <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="asyncAction">The asynchronous action to perform.</param>
    /// <returns>A task which represents an asynchronous operation.</returns>
    public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> asyncAction)
    {
        foreach (var element in enumerable)
        {
            await asyncAction(element).ConfigureAwait(false);
        }
    }
}
