using System.Threading.Tasks;

namespace Gybs.Logic.Operations
{
    /// <summary>
    /// Represents a handler of an operation.
    /// </summary>
    /// <typeparam name="TOperation">The type of command to handle.</typeparam>
    public interface IOperationHandler<in TOperation>
        where TOperation: IOperation, new()
    {        
        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="operation">The operation to handle.</param>
        /// <returns>The result.</returns>
        Task<IResult> HandleAsync(TOperation operation);
    }

    /// <summary>
    /// Represents a handler of an operation.
    /// </summary>
    /// <typeparam name="TOperation">The type of command to handle.</typeparam>
    /// <typeparam name="TData">The type of returned data.</typeparam>
    public interface IOperationHandler<in TOperation, TData>
        where TOperation : IOperation<TData>, new()
    {
        /// <summary>
        /// Handles the operation.
        /// </summary>
        /// <param name="operation">The operation to handle.</param>
        /// <returns>The result with returned data.</returns>
        Task<IResult<TData>> HandleAsync(TOperation operation);
    }
}
