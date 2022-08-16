using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gybs.Logic.Operations.Factory.Internal;

internal class OperationFactory : IOperationFactory
{
    private readonly List<IOperationInitializer> _operationInitializers;
    private readonly IOperationBus _operationBus;

    public OperationFactory(
        ILogger<OperationFactory> logger,
        IEnumerable<IOperationInitializer> operationInitializers,
        IOperationBus operationBus)
    {
        _operationInitializers = operationInitializers.ToList();
        _operationBus = operationBus;
        logger.LogDebug($"Resolved {_operationInitializers.Count} operation initializers.");
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

        return CreateProxy(new TOperation(), initializer);
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

        return CreateProxy(operation, initializer);
    }

    private IOperationProxy<TOperation> CreateProxy<TOperation>(TOperation operation, Action<TOperation>? initializer)
        where TOperation : IOperationBase, new()
    {
        _operationInitializers.ForEach(i => i.Initialize(operation));
        initializer?.Invoke(operation);

        return new OperationProxy<TOperation>(operation, _operationBus);
    }
}
