using System.Threading.Tasks;

namespace Gybs.Logic.Operations;

/// <summary>
/// Represents a bus responsible for operation handling.
/// </summary>
public interface IOperationBus
{
    /// <summary>
    /// Handles the operation.
    /// </summary>
    /// <param name="operation">The operation to handle.</param>
    /// <returns>The result.</returns>
    Task<IResult> HandleAsync(IOperation operation);

    /// <summary>
    /// Handles the operation.
    /// </summary>
    /// <param name="operation">The operation to handle.</param>
    /// <returns>The result with data.</returns>
    Task<IResult<TData>> HandleAsync<TData>(IOperation<TData> operation);
}
