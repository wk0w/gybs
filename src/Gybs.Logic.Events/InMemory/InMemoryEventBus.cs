using Gybs.Extensions;
using Gybs.Logic.Events.Subscriptions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gybs.Logic.Events.InMemory
{
    /// <summary>
    /// Implements <see cref="IEventBus" /> using memory as a transport layer.
    /// </summary>
    /// <remarks>
    /// Sent events will be immediately handled by subscribers on the same thread the call
    /// was executed, one by one.
    /// </remarks>
    public sealed class InMemoryEventBus : IEventBus, IDisposable
    {        
        private readonly ILogger<InMemoryEventBus> _logger;
        private readonly SubscriptionsCollection _subscriptions = new SubscriptionsCollection();
        private readonly object _disposeLock = new object();
        private bool _isDisposed;

        public InMemoryEventBus(ILogger<InMemoryEventBus> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Disposes the object, canceling all subscriptions.
        /// </summary>
        public void Dispose()
        {
            lock (_disposeLock)
            {
                if (_isDisposed) throw new ObjectDisposedException(nameof(InMemoryEventBus));
                _isDisposed = true;                
            }

            _subscriptions.Dispose();
        }

        /// <summary>
        /// Sends an event to all subscribers.
        /// </summary>
        /// <typeparam name="TEvent">Type of the event.</typeparam>
        /// <param name="evnt">The event.</param>
        /// <returns>A task which represents an asynchronous operation.</returns>
        public async Task SendAsync<TEvent>(TEvent evnt)
            where TEvent : class, IEvent
        {
            lock (_disposeLock)
            {
                if (_isDisposed) throw new ObjectDisposedException(nameof(InMemoryEventBus));
            }

            _logger.LogDebug($"Received the event of type '{typeof(TEvent)}'.");

            foreach (var subscription in _subscriptions.GetCopy<TEvent>())
            {
                try
                {
                    await subscription.InvokeAsync(evnt);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Action for '{typeof(TEvent).FullName}' failed.");
                }
            }
        }

        /// <summary>
        /// Subscribes to an event.
        /// </summary>
        /// <typeparam name="TEvent">Type of the event.</typeparam>
        /// <param name="action">Action to perform when event is received.</param>
        /// <returns>Cancellation token source, which can be used to cancel the subscription.</returns>
        public Task<CancellationTokenSource> SubscribeAsync<TEvent>(Func<TEvent, Task> action)
            where TEvent : class, IEvent
        {
            lock (_disposeLock)
            {
                if (_isDisposed) throw new ObjectDisposedException(nameof(InMemoryEventBus));
            }

            _logger.LogDebug($"Subscribing for '{typeof(TEvent)}'.");
            var subscription = new Subscription<TEvent>(action);            
            subscription.CancellationTokenSource.Token
                .Register(() =>
                {
                    lock (_disposeLock)
                    {
                        if (_isDisposed) return;
                    }

                    _logger.LogDebug($"Removing subscription for '{typeof(TEvent)}'.");
                    _subscriptions.Remove(subscription);
                });

            _subscriptions.Add(subscription);
            
            return subscription.CancellationTokenSource.ToCompletedTask();
        }
    }    
}
