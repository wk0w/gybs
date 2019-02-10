using System.Threading.Tasks;

namespace Gybs.Extensions
{
    /// <summary>
    /// <see cref="Task{TResult}"/> extensions.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Wraps the object into a task with <see cref="Task.FromResult{TResult}"/>.
        /// </summary>
        /// <typeparam name="TObject">The object type.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>The completed task.</returns>
        public static Task<TObject> ToCompletedTask<TObject>(this TObject obj)
        {
            return Task.FromResult(obj);
        }
    }
}
