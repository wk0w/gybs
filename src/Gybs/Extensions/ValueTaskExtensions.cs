using System.Threading.Tasks;

namespace Gybs.Extensions;

/// <summary>
/// <see cref="ValueTask{TResult}"/> extensions.
/// </summary>
public static class ValueTaskExtensions
{
    /// <summary>
    /// Wraps the object into a value task with <see cref="ValueTask.FromResult{TResult}"/>.
    /// </summary>
    /// <typeparam name="TObject">The object type.</typeparam>
    /// <param name="obj">The object.</param>
    /// <returns>The completed value task.</returns>
    public static ValueTask<TObject> ToCompletedValueTask<TObject>(this TObject obj) => ValueTask.FromResult(obj);
}
