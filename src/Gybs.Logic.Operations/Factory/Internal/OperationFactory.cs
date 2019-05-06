using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Gybs.Logic.Operations.Factory.Internal
{
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
            return Create<TOperation>(null);
        }

        public IOperationProxy<TOperation> Create<TOperation>(Action<TOperation> initializer)
            where TOperation : IOperationBase, new()
        {
            var operation = new TOperation();

            foreach (var operationInitializer in _operationInitializers)
            {
                operationInitializer.Initialize(operation);
            }

            initializer?.Invoke(operation);
            return new OperationProxy<TOperation>(operation, _operationBus);
        }
    }
}
