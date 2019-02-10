using System.Threading;
using System.Threading.Tasks;

namespace Gybs.Logic.Events
{
    /// <summary>
    /// Represents a subscription for an event.
    /// </summary>
    public interface ISubscription
    {
        /// <summary>
        /// Gets the token source for cancellation of the subscription.
        /// </summary>
        CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// Invokes the action associated with the subscription.
        /// </summary>
        /// <param name="evnt">The event to handle.</param>
        /// <returns>A task which represents and asynchronous operation.</returns>
        Task InvokeAsync(IEvent evnt);
    }
}
