using System.Threading.Tasks;

namespace Gybs.Logic.Cqrs
{
    /// <summary>
    /// Represents a handler of a command.
    /// </summary>
    /// <typeparam name="TCommand">The type of command to handle.</typeparam>
    public interface ICommandHandler<in TCommand>
        where TCommand: ICommand, new()
    {        
        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <returns>The result.</returns>
        Task<IResult> HandleAsync(TCommand command);
    }

    /// <summary>
    /// Represents a handler of a command.
    /// </summary>
    /// <typeparam name="TCommand">The type of command to handle.</typeparam>
    /// <typeparam name="TData">The type of returned data.</typeparam>
    public interface ICommandHandler<in TCommand, TData>
        where TCommand : ICommand<TData>, new()
    {
        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <returns>The result with returned data.</returns>
        Task<IResult<TData>> HandleAsync(TCommand command);
    }
}
