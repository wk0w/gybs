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
        _factory = new OperationFactory(
            logger,
            new[] { new DummyOperationInitializer(), new DummyOperationInitializer() },
            new[] { new DummyImmutableOperationInitializer(), new DummyImmutableOperationInitializer() },
            bus
        );
    }

    [Fact]
    public void ForCreatedOperationShouldCreateProxy()
    {
        var proxy = _factory.Create<DummyClassOperation>();

        proxy.Should().NotBeNull();
        proxy.Operation.Should().NotBeNull();
        proxy.Bus.Should().NotBeNull();
    }

    [Fact]
    public void ForCreatedOperationShouldUseInitializers()
    {
        var proxy = _factory.Create<DummyClassOperation>(o => o.Str = o.Str == "##" ? "test" : null);

        proxy.Operation.Str.Should().Be("test");
    }

    [Fact]
    public void ForExistingOperationWithInitializerMethodShouldCreateProxy()
    {
        var operation = new DummyClassOperation();
        var proxy = _factory.UseExisting(operation);

        proxy.Should().NotBeNull();
        proxy.Operation.Should().Be(operation);
        proxy.Bus.Should().NotBeNull();
    }

    [Fact]
    public void ForExistingOperationWithInitializerMethodShouldUseInitializers()
    {
        var operation = new DummyClassOperation();
        var proxy = _factory.UseExisting(operation, o => o.Str = o.Str == "##" ? "test" : null);

        proxy.Operation.Str.Should().Be("test");
    }

    [Fact]
    public void ForExistingOperationWithFactoryMethodShouldCreateProxy()
    {
        var operation = new DummyRecordOperation();
        DummyRecordOperation? finalOperation = null;
        var proxy = _factory.UseExisting(operation, o =>
        {
            finalOperation = o with { Str = "x" };
            return finalOperation;
        });

        proxy.Should().NotBeNull();
        proxy.Operation.Should().Be(finalOperation);
        proxy.Bus.Should().NotBeNull();
    }

    [Fact]
    public void ForExistingOperationWithFactoryMethodShouldUseInitializers()
    {
        var operation = new DummyRecordOperation();
        var proxy = _factory.UseExisting(operation, o => o with { Str = o.Str == "##" ? "test" : null });

        proxy.Operation.Str.Should().Be("test");
    }

    private class DummyClassOperation : IOperation
    {
        public string? Str { get; set; }
    }

    private record DummyRecordOperation : IOperation
    {
        public string? Str { get; init; }
        public int? Int { get; init; }
    }

    private class DummyOperationInitializer : IOperationInitializer
    {
        public void Initialize(IOperationBase operation)
        {
            if (operation is DummyClassOperation dummyOperation)
            {
                dummyOperation.Str += "#";
            }
        }
    }

    private class DummyImmutableOperationInitializer : IImmutableOperationInitializer
    {
        public IOperationBase Initialize(IOperationBase operation)
        {
            if (operation is DummyRecordOperation dummyOperation)
            {
                return dummyOperation with { Str = dummyOperation.Str + "#" };
            }

            return operation;
        }
    }
}
