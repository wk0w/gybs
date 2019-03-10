using Gybs.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Gybs.Logic.Events.Internal
{
    internal class SubscriptionsCollection : IDisposable
    {
        private readonly ConcurrentDictionary<Type, List<ISubscription>> _subscriptions = new ConcurrentDictionary<Type, List<ISubscription>>();

        public void Dispose()
        {
            _subscriptions.SelectMany(s => s.Value)
                .ForEach(s => s.CancellationTokenSource.Cancel());
            _subscriptions.Clear();
        }

        public void Add<TEvent>(Subscription<TEvent> subscription)
        {
            var subscriptions = GetOrAddSubscriptions<TEvent>();

            lock (subscriptions)
            {
                subscriptions.Add(subscription);
            }
        }

        public void Remove<TEvent>(Subscription<TEvent> subscription)
        {
            var subscriptions = GetOrAddSubscriptions<TEvent>();

            lock (subscriptions)
            {
                subscriptions.Remove(subscription);
            }
        }

        public IReadOnlyCollection<ISubscription> GetCopy<TEvent>()
        {
            var subscriptions = GetOrAddSubscriptions<TEvent>();

            lock (subscriptions)
            {
                return new List<ISubscription>(subscriptions);
            }
        }
    
        private List<ISubscription> GetOrAddSubscriptions<TEvent>() => _subscriptions.GetOrAdd(typeof(TEvent), t => new List<ISubscription>());
    }
}
