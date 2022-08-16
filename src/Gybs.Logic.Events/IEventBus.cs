using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gybs.Logic.Events;

/// <summary>
/// Represents a bus for the events.
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Sends an event to all subscribers.
    /// </summary>
    /// <typeparam name="TEvent">Type of the event.</typeparam>
    /// <param name="evnt">The event.</param>
    /// <returns>A task which represents an asynchronous operation.</returns>
    Task SendAsync<TEvent>(TEvent evnt)
        where TEvent : class, IEvent;

    /// <summary>
    /// Subscribes to an event.
    /// </summary>
    /// <typeparam name="TEvent">Type of the event.</typeparam>
    /// <param name="action">Action to perform when event is received.</param>
    /// <param name="additionalParams">Collection with additional parameters passed to the bus.</param>
    /// <returns>Cancellation token source, which can be used to cancel the subscription.</returns>
    Task<CancellationTokenSource> SubscribeAsync<TEvent>(Func<TEvent, Task> action, IReadOnlyDictionary<string, string>? additionalParams = null)
        where TEvent : class, IEvent;
}
