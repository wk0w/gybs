using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gybs.Logic.Operations.Factory.Internal;

internal class OperationFactory : IOperationFactory
{
    private readonly List<IOperationInitializer> _operationInitializers;
    private readonly List<IImmutableOperationInitializer> _immutableOperationInitializers;
    private readonly IOperationBus _operationBus;

    public OperationFactory(
        ILogger<OperationFactory> logger,
        IEnumerable<IOperationInitializer> operationInitializers,
        IEnumerable<IImmutableOperationInitializer> immutableOperationInitializers,
        IOperationBus operationBus)
    {
        _operationInitializers = operationInitializers.ToList();
        _immutableOperationInitializers = immutableOperationInitializers.ToList();
        _operationBus = operationBus;
        logger.LogDebug($"Resolved {_operationInitializers.Count + _immutableOperationInitializers.Count} operation initializers.");
    }

    public IOperationProxy<TOperation> Create<TOperation>()
        where TOperation : IOperationBase, new()
    {
        return CreateProxy(new TOperation(), null);
    }

    public IOperationProxy<TOperation> Create<TOperation>(Action<TOperation> initializer)
        where TOperation : IOperationBase, new()
    {
        if (initializer is null) throw new ArgumentNullException(nameof(initializer));

        return CreateProxy(new TOperation(), o =>
        {
            initializer.Invoke(o);
            return o;
        });
    }

    public IOperationProxy<TOperation> UseExisting<TOperation>(TOperation operation)
        where TOperation : IOperationBase, new()
    {
        if (operation is null) throw new ArgumentNullException(nameof(operation));

        return CreateProxy(operation, null);
    }

    public IOperationProxy<TOperation> UseExisting<TOperation>(TOperation operation, Action<TOperation> initializer)
        where TOperation : IOperationBase, new()
    {
        if (operation is null) throw new ArgumentNullException(nameof(operation));
        if (initializer is null) throw new ArgumentNullException(nameof(initializer));

        return CreateProxy(operation, o =>
        {
            initializer.Invoke(o);
            return o;
        });
    }

    public IOperationProxy<TOperation> UseExisting<TOperation>(TOperation operation, Func<TOperation, TOperation> factory)
        where TOperation : IOperationBase, new()
    {
        if (operation is null) throw new ArgumentNullException(nameof(operation));
        if (factory is null) throw new ArgumentNullException(nameof(factory));

        return CreateProxy(operation, factory);
    }

    private IOperationProxy<TOperation> CreateProxy<TOperation>(TOperation operation, Func<TOperation, TOperation>? factory)
        where TOperation : IOperationBase, new()
    {
        _operationInitializers.ForEach(i => i.Initialize(operation));
        var initializedOperation = _immutableOperationInitializers.Aggregate(operation, (o, i) => (TOperation)i.Initialize(o));
        initializedOperation = factory is not null ? factory.Invoke(initializedOperation) : initializedOperation;

        return new OperationProxy<TOperation>(initializedOperation, _operationBus);
    }
}
