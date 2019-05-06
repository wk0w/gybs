using FluentAssertions;
using Gybs.Logic.Events;
using Gybs.Logic.Events.InMemory;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Gybs.Tests.Logic.Events.InMemory
{
    public class InMemoryEventBusTests
    {
        [Fact]
        public async Task ForSendEventActionsAreInvoked()
        {
            var bus = CreateBus();
            var count = 0;
            const int expectedCount = 3;

            for (var i = 0; i < expectedCount; i++)
            {
                await bus.SubscribeAsync<Event>(_ => { count++; return Task.CompletedTask; });
            }
            await bus.SendAsync(new Event());

            count.Should().Be(expectedCount);
        }

        [Fact]
        public async Task ForFailedActionOtherActionsAreInvoked()
        {
            var bus = CreateBus();
            var count = 0;

            await bus.SubscribeAsync<Event>(_ => { count++; return Task.CompletedTask; });
            await bus.SubscribeAsync<Event>(_ => throw new InvalidOperationException());
            await bus.SubscribeAsync<Event>(_ => { count++; return Task.CompletedTask; });
            await bus.SendAsync(new Event());

            count.Should().Be(2);
        }

        [Fact]
        public async Task ForCanceledSubscriptionActionIsNotInvoked()
        {
            var bus = CreateBus();
            var count = 0;

            var subscription = await bus.SubscribeAsync<Event>(_ => { count++; return Task.CompletedTask; });
            subscription.Cancel();
            await bus.SendAsync(new Event());

            count.Should().Be(0);
        }

        [Fact]
        public async Task ForNoSubscribersEventIsSend()
        {
            var bus = CreateBus();
            await bus.SendAsync(new Event());
        }

        private InMemoryEventBus CreateBus() => new InMemoryEventBus(Substitute.For<ILogger<InMemoryEventBus>>());

        private class Event : IEvent
        {
        }
    }
}