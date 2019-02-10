using System;
using System.Threading.Tasks;
using Gybs.Logic.Cqrs.Factory.Internal;
using Gybs.Logic.Cqrs.Internal;

namespace Gybs.Logic.Cqrs.Factory
{
    /// <summary>
    /// Extensions for <see cref="IOperationProxy{TOperation}"/>.
    /// </summary>
    public static class OperationProxyExtensions
    {
        /// <summary>
        /// Handles query.
        /// </summary>
        /// <param name="operationProxy">Operation proxy.</param>
        /// <returns>Result with data.</returns>
        public static Task<IResult<TData>> HandleAsync<TData>(this IOperationProxy<IQuery<TData>> operationProxy)
        {
            return operationProxy.Bus.HandleAsync(operationProxy.Operation);
        }

        /// <summary>
        /// Handles command.
        /// </summary>
        /// <param name="operationProxy">Operation proxy.</param>
        /// <returns>Result.</returns>
        public static Task<IResult> HandleAsync(this IOperationProxy<ICommand> operationProxy)
        {
            return operationProxy.Bus.HandleAsync(operationProxy.Operation);
        }

        /// <summary>
        /// Handles command.
        /// </summary>
        /// <param name="operationProxy">Operation proxy.</param>
        /// <returns>Result with data.</returns>
        public static Task<IResult<TData>> HandleAsync<TData>(this IOperationProxy<ICommand<TData>> operationProxy)
        {
            return operationProxy.Bus.HandleAsync(operationProxy.Operation);
        }                
    }
}