using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Gybs.Logic.Cqrs.Internal
{
    internal class SelfResolvingOperationBus : IOperationBus
    {
        private static readonly Type QueryHandlerType = typeof(IQueryHandler<,>);
        private static readonly Type CommandHandlerType = typeof(ICommandHandler<>);
        private static readonly Type DataCommandHandlerType = typeof(ICommandHandler<,>);

        private static readonly ConcurrentDictionary<Type, OperationHandlerInvoker> Invokers = new ConcurrentDictionary<Type, OperationHandlerInvoker>();

        private readonly ILogger<SelfResolvingOperationBus> _logger;
        private readonly IServiceProvider _serviceProvider;

        public SelfResolvingOperationBus(
            ILogger<SelfResolvingOperationBus> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task<IResult<TData>> HandleAsync<TData>(IQuery<TData> query)
        {
            return HandleAsync(query, QueryHandlerType);
        }

        public Task<IResult> HandleAsync(ICommand command)
        {
            return HandleAsync(command, CommandHandlerType);
        }

        public Task<IResult<TData>> HandleAsync<TData>(ICommand<TData> command)
        {
            return HandleAsync(command, DataCommandHandlerType);
        }

        private Task<IResult> HandleAsync(IOperation operation, Type operationHandlerType)
        {
            var operationType = operation.GetType();
            var operationHandlerInvoker = Invokers.GetOrAdd(operationType, t => CreateOperationHandlerInvoker(operationHandlerType, operationType));
            var operationHandler = _serviceProvider.GetService(operationHandlerInvoker.OperationHandlerType);

            if (operationHandler == null)
            {
                throw new InvalidOperationException($"No handler for operation of type {operationType.FullName}.");
            }

            _logger.LogDebug($"Invoking {operationHandlerInvoker.OperationHandlerType.FullName} for {operationType.FullName}.");
            return operationHandlerInvoker.InvokeAsync(operationHandler, operation);            
        }

        private Task<IResult<TData>> HandleAsync<TData>(IOperation<TData> operation, Type operationHandlerType)
        {
            var operationType = operation.GetType();
            var operationHandlerInvoker = Invokers.GetOrAdd(operationType, t => CreateOperationHandlerInvoker(operationHandlerType, operationType, typeof(TData)));
            var operationHandler = _serviceProvider.GetService(operationHandlerInvoker.OperationHandlerType);

            if (operationHandler == null)
            {
                throw new InvalidOperationException($"No handler for operation of type {operationType.FullName}.");
            }

            _logger.LogDebug($"Invoking {operationHandlerInvoker.OperationHandlerType.FullName} for {operationType.FullName}.");
            return operationHandlerInvoker.InvokeAsync<TData>(operationHandler, operation);
        }

        private OperationHandlerInvoker CreateOperationHandlerInvoker(Type operationHandlerType, Type operationType)
        {
            var type = operationHandlerType.MakeGenericType(operationType);
            _logger.LogDebug($"Generated handler {type.FullName} for {operationType.FullName}.");
            return new OperationHandlerInvoker(type);
        }

        private OperationHandlerInvoker CreateOperationHandlerInvoker(Type operationHandlerType, Type operationType, Type dataType)
        {
            var type = operationHandlerType.MakeGenericType(operationType, dataType);
            _logger.LogDebug($"Generated handler {type.FullName} for {operationType.FullName}.");
            return new OperationHandlerInvoker(type);
        }
    }
}
