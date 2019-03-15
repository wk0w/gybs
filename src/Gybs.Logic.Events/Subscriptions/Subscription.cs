using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gybs.Logic.Events.Subscriptions
{
    /// <summary>
    /// Represents a subscription for specific type of <see cref="IEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public sealed class Subscription<TEvent> : ISubscription
    {
        private readonly Func<TEvent, Task> _action;
        
        /// <summary>
        /// Gets token source for cancellation of the subscription.
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }
            
        /// <summary>
        /// Creates new instance of subscription.
        /// </summary>
        /// <param name="action">Action to invoke when event is received.</param>
        public Subscription(Func<TEvent, Task> action)
        {
            _action = action;
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Invokes an action associated with the subscription.
        /// </summary>
        /// <param name="evnt">The event to handle.</param>
        /// <returns>A task which represents and asynchronous operation.</returns>
        public Task InvokeAsync(IEvent evnt)
        {
            if (!(evnt is TEvent castedEvent))
            {
                return Task.CompletedTask;
            }

            return _action(castedEvent);
        }
    }
}
