using System.Threading.Tasks;

namespace Gybs.Logic.Cqrs
{
    /// <summary>
    /// Represents a bus for operation handling.
    /// </summary>
    public interface IOperationBus
    {
        /// <summary>
        /// Handles the query.
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="query">The query to handle.</param>
        /// <returns>The result with data.</returns>
        Task<IResult<TData>> HandleAsync<TData>(IQuery<TData> query);

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <returns>The result.</returns>
        Task<IResult> HandleAsync(ICommand command);

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <returns>The result with data.</returns>
        Task<IResult<TData>> HandleAsync<TData>(ICommand<TData> command);        
    }
}
