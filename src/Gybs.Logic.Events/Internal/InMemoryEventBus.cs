using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gybs.Extensions;
using Microsoft.Extensions.Logging;

namespace Gybs.Logic.Events.Internal
{
    internal class InMemoryEventBus : IEventBus
    {
        private readonly ILogger<InMemoryEventBus> _logger;
        private readonly ConcurrentDictionary<Type, List<ISubscription>> _subscriptions = new ConcurrentDictionary<Type, List<ISubscription>>();

        public InMemoryEventBus(ILogger<InMemoryEventBus> logger)
        {
            _logger = logger;
        }

        public Task SendAsync<TEvent>(TEvent evnt)
            where TEvent : class, IEvent
        {
            _logger.LogDebug($"Received event of type {typeof(TEvent)}.");
            var subscriptions = GetSubscriptions<TEvent>().ToArray();
            return Task.WhenAll(subscriptions.Select(s => s.InvokeAsync(evnt)));
        }

        public Task<CancellationTokenSource> SubscribeAsync<TEvent>(Func<TEvent, Task> action)
            where TEvent : class, IEvent
        {
            _logger.LogDebug($"Subscribing for {typeof(TEvent)}.");
            var subscription = new Subscription<TEvent>(action);
            var subscriptions = GetSubscriptions<TEvent>();

            subscription.CancellationTokenSource.Token
                .Register(() =>
                {
                    _logger.LogDebug($"Removing subscription for {typeof(TEvent)}.");
                    _subscriptions[typeof(TEvent)].Remove(subscription);
                    subscription.CancellationTokenSource.Dispose();
                });

            subscriptions.Add(subscription);

            return subscription.CancellationTokenSource.ToCompletedTask();
        }

        private List<ISubscription> GetSubscriptions<TEvent>() => _subscriptions.GetOrAdd(typeof(TEvent), t => new List<ISubscription>());
    }
}
