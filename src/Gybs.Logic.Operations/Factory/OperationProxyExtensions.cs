using System;
using System.Threading.Tasks;
using Gybs.Logic.Operations.Factory.Internal;

namespace Gybs.Logic.Operations.Factory
{
    /// <summary>
    /// Extensions for <see cref="IOperationProxy{TOperation}"/>.
    /// </summary>
    public static class OperationProxyExtensions
    {
        /// <summary>
        /// Handles the operation.
        /// </summary>
        /// <param name="operationProxy">Operation proxy.</param>
        /// <returns>Result.</returns>
        public static Task<IResult> HandleAsync(this IOperationProxy<IOperation> operationProxy)
        {
            return operationProxy.Bus.HandleAsync(operationProxy.Operation);
        }

        /// <summary>
        /// Handles the operation.
        /// </summary>
        /// <param name="operationProxy">Operation proxy.</param>
        /// <returns>Result with data.</returns>
        public static Task<IResult<TData>> HandleAsync<TData>(this IOperationProxy<IOperation<TData>> operationProxy)
        {
            return operationProxy.Bus.HandleAsync(operationProxy.Operation);
        }
    }
}
