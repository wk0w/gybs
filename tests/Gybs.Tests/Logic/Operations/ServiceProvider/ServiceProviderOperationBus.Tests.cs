using System;
using System.Threading.Tasks;
using FluentAssertions;
using Gybs.Extensions;
using Gybs.Logic.Operations;
using Gybs.Logic.Operations.ServiceProvider;
using Gybs.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Gybs.Tests.Logic.ServiceProvider
{
    public class ServiceProviderOperationBusTests
    {
        [Fact]
        public async Task ForRegisteredHandlerShouldHandleOperationWithoutData()
        {
            var bus = CreateBus();
            var result = await bus.HandleAsync(new DummyOperation());
            result.HasSucceeded.Should().BeTrue();
        }

        [Fact]
        public async Task ForRegisteredHandlerShouldHandleOperationWithData()
        {
            var bus = CreateBus();
            var result = await bus.HandleAsync(new DummyDataOperation { InputData = 1 });
            result.HasSucceeded.Should().BeTrue();
            result.Data.Should().Be(1);
        }

        [Fact]
        public async Task ForRegisteredHandlerShouldCreateNewHandler()
        {
            var bus = CreateBus();
            _ = await bus.HandleAsync(new DummyDataOperation { InputData = 1 });
            IResult<int> result = await bus.HandleAsync(new DummyDataOperation { InputData = 1 });
            result.HasSucceeded.Should().BeTrue();
            result.Data.Should().Be(1);
        }

        private IOperationBus CreateBus()
        {
            var logger = Substitute.For<ILogger<ServiceProviderOperationBus>>();
            var serviceProvider = new DefaultServiceProviderFactory().CreateServiceProvider(
                new ServiceCollection().AddGybs(builder => builder.AddOperationHandlers())
            );

            return new ServiceProviderOperationBus(logger, serviceProvider);
        }

        private class DummyOperation : IOperation
        {
        }

        private class DummyOperationHandler : IOperationHandler<DummyOperation>
        {
            public Task<IResult> HandleAsync(DummyOperation operation) => Result.Success().ToCompletedTask();
        }

        private class DummyDataOperation : IOperation<int>
        {
            public int InputData { get; set; }
        }

        private class DummyDataOperationHandler : IOperationHandler<DummyDataOperation, int>
        {
            private int _counter = 0;

            public Task<IResult<int>> HandleAsync(DummyDataOperation operation) => Result.Success(operation.InputData + _counter++).ToCompletedTask();
        }
    }
}