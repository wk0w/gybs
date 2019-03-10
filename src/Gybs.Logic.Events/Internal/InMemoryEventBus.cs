using Gybs.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gybs.Logic.Events.Internal
{
    internal partial class InMemoryEventBus : IEventBus, IDisposable
    {        
        private readonly ILogger<InMemoryEventBus> _logger;
        private readonly SubscriptionsCollection _subscriptions = new SubscriptionsCollection();
        private readonly object _disposeLock = new object();
        private bool _isDisposed;

        public InMemoryEventBus(ILogger<InMemoryEventBus> logger)
        {
            _logger = logger;
        }

        public void Dispose()
        {
            lock (_disposeLock)
            {
                if (_isDisposed) throw new ObjectDisposedException(nameof(InMemoryEventBus));
                _isDisposed = true;                
            }

            _subscriptions.Dispose();
        }

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
