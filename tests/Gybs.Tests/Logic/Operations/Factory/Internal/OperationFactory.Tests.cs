using FluentAssertions;
using Gybs.Logic.Operations;
using Gybs.Logic.Operations.Factory;
using Gybs.Logic.Operations.Factory.Internal;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Gybs.Tests.Logic.Operations.Factory.Internal;

public class OperationFactoryTests
{
    private readonly OperationFactory _factory;

    public OperationFactoryTests()
    {
        var logger = Substitute.For<ILogger<OperationFactory>>();
        var bus = Substitute.For<IOperationBus>();
        _factory = new OperationFactory(logger, new[] { new DummyOperationInitializer() }, bus);
    }

    [Fact]
    public void ForCreatedOperationShouldCreateProxy()
    {
        var proxy = _factory.Create<DummyOperation>();

        proxy.Should().NotBeNull();
        proxy.Operation.Should().NotBeNull();
        proxy.Bus.Should().NotBeNull();
    }

    [Fact]
    public void ForCreatedOperationShouldUseInitializers()
    {
        var proxy = _factory.Create<DummyOperation>(o => o.Str = o.Str == "test" ? "test2" : null);

        proxy.Operation.Str.Should().Be("test2");
    }

    [Fact]
    public void ForExistingOperationShouldCreateProxy()
    {
        var operation = new DummyOperation();
        var proxy = _factory.UseExisting(operation);

        proxy.Should().NotBeNull();
        proxy.Operation.Should().Be(operation);
        proxy.Bus.Should().NotBeNull();
    }

    [Fact]
    public void ForExistingOperationShouldUseInitializers()
    {
        var operation = new DummyOperation();
        var proxy = _factory.UseExisting(operation, o => o.Str = o.Str == "test" ? "test2" : null);

        proxy.Operation.Str.Should().Be("test2");
    }

    private class DummyOperation : IOperation
    {
        public string? Str { get; set; }
    }

    private class DummyOperationInitializer : IOperationInitializer
    {
        public void Initialize(IOperationBase operation)
        {
            if (operation is DummyOperation dummyOperation)
            {
                dummyOperation.Str = "test";
            }
        }
    }
}
