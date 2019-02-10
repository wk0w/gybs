using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Gybs.Logic.Cqrs.Internal
{
    internal class OperationHandlerInvoker
    {
        private readonly MethodInfo _handleAsyncMethodInfo;

        public Type OperationHandlerType { get; }

        public OperationHandlerInvoker(Type operationHandlerType)
        {
            OperationHandlerType = operationHandlerType;
            _handleAsyncMethodInfo = operationHandlerType.GetMethod("HandleAsync");
        }

        public Task<IResult> InvokeAsync(object operationHandler, object operation)
        {
            return (Task<IResult>)_handleAsyncMethodInfo.Invoke(operationHandler, new[] { operation });
        }

        public Task<IResult<TData>> InvokeAsync<TData>(object operationHandler, object operation)
        {
            return (Task<IResult<TData>>)_handleAsyncMethodInfo.Invoke(operationHandler, new[] { operation });
        }        
    }
}
