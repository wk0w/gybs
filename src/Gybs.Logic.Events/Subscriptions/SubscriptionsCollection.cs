using Gybs.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Gybs.Logic.Events.Subscriptions;

/// <summary>
/// Represents a thread-safe collection of the subscriptions grouped by the events.
/// </summary>
public sealed class SubscriptionsCollection : IDisposable
{
    private readonly ConcurrentDictionary<Type, List<ISubscription>> _subscriptions = new();
    private readonly object _disposeLock = new();
    private bool _isDisposed;

    /// <summary>
    /// Disposes the collection and cancels all the cancellation tokens.
    /// </summary>
    public void Dispose()
    {
        lock (_disposeLock)
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(SubscriptionsCollection));
            _isDisposed = true;
        }

        _subscriptions.SelectMany(s => s.Value)
            .ForEach(s => s.CancellationTokenSource.Cancel());
        _subscriptions.Clear();
    }

    /// <summary>
    /// Adds a new subscription to the collection.
    /// </summary>
    /// <param name="subscription">Subscription to add.</param>
    /// <typeparam name="TEvent">Type of an event.</typeparam>
    public void Add<TEvent>(Subscription<TEvent> subscription)
    {
        lock (_disposeLock)
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(SubscriptionsCollection));
        }

        var subscriptions = GetOrAddSubscriptions<TEvent>();

        lock (subscriptions)
        {
            subscriptions.Add(subscription);
        }
    }

    /// <summary>
    /// Removes the subscription from the collection.
    /// </summary>
    /// <param name="subscription">Subscription to add.</param>
    /// <typeparam name="TEvent">Type of an event.</typeparam>
    public void Remove<TEvent>(Subscription<TEvent> subscription)
    {
        lock (_disposeLock)
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(SubscriptionsCollection));
        }

        var subscriptions = GetOrAddSubscriptions<TEvent>();

        lock (subscriptions)
        {
            subscriptions.Remove(subscription);
        }
    }

    /// <summary>
    /// Gets the collection of subscriptions associated with the event.
    /// </summary>
    /// <typeparam name="TEvent">Type of an event.</typeparam>
    /// <returns>New collection.</returns>
    public IReadOnlyCollection<ISubscription> GetCopy<TEvent>()
    {
        lock (_disposeLock)
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(SubscriptionsCollection));
        }

        var subscriptions = GetOrAddSubscriptions<TEvent>();

        lock (subscriptions)
        {
            return new List<ISubscription>(subscriptions);
        }
    }

    private List<ISubscription> GetOrAddSubscriptions<TEvent>() => _subscriptions.GetOrAdd(typeof(TEvent), _ => new List<ISubscription>());
}
