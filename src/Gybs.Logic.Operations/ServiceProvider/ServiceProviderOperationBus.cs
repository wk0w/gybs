using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Gybs.Logic.Operations.ServiceProvider
{
    /// <summary>
    /// Represents a bus handling operation via handlers resolved from a service provider.
    /// </summary>
    public class ServiceProviderOperationBus : IOperationBus
    {
        private static readonly Type OperationHandlerType = typeof(IOperationHandler<>);
        private static readonly Type DataOperationHandlerType = typeof(IOperationHandler<,>);
        private static readonly ConcurrentDictionary<Type, (Type type, MethodInfo method)> HandlerDefinitions = new ConcurrentDictionary<Type, (Type, MethodInfo)>();

        private readonly ILogger<ServiceProviderOperationBus> _logger;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates an instance of the bus.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="serviceProvider">Service provider used to resolve the handlers.</param>
        public ServiceProviderOperationBus(
            ILogger<ServiceProviderOperationBus> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Handles the operation.
        /// </summary>
        /// <param name="operation">The operation to handle.</param>
        /// <returns>The result.</returns>
        public Task<IResult> HandleAsync(IOperation operation)
        {
            return (Task<IResult>)Handle(operation, OperationHandlerType, null);
        }

        /// <summary>
        /// Handles the operation.
        /// </summary>
        /// <param name="operation">The operation to handle.</param>
        /// <returns>The result with data.</returns>
        public Task<IResult<TData>> HandleAsync<TData>(IOperation<TData> operation)
        {
            return (Task<IResult<TData>>)Handle(operation, DataOperationHandlerType, typeof(TData));
        }

        private object Handle(IOperationBase operation, Type operationHandlerType, Type? dataType)
        {
            var operationType = operation.GetType();
            var operationHandlerDefinition = HandlerDefinitions.GetOrAdd(operationType, _ => CreateHandler(operationHandlerType, operationType, dataType));
            var operationHandler = _serviceProvider.GetService(operationHandlerDefinition.type);

            if (operationHandler is null) throw new InvalidOperationException($"No handler for operation of type {operationType.FullName}.");

            _logger.LogDebug($"Invoking {operationHandler.GetType().FullName} for {operationType.FullName}.");
            return operationHandlerDefinition.method.Invoke(operationHandler, new[] { operation });
        }

        private (Type, MethodInfo) CreateHandler(Type handlerType, Type operationType, Type? dataType)
        {
            var type = dataType is { }
                ? handlerType.MakeGenericType(operationType, dataType)
                : handlerType.MakeGenericType(operationType);
            _logger.LogDebug($"Generated handler {type.FullName} for {operationType.FullName}.");

            return (type, type.GetMethod("HandleAsync"));
        }
    }
}
