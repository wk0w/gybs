using Gybs.Logic.Operations;

namespace Gybs.Logic.Cqrs
{
    /// <summary>
    /// Represents a handler of a command.
    /// </summary>
    /// <typeparam name="TCommand">The type of command to handle.</typeparam>
    public interface ICommandHandler<in TCommand> : IOperationHandler<TCommand>
        where TCommand : ICommand, new()
    {
    }

    /// <summary>
    /// Represents a handler of a command.
    /// </summary>
    /// <typeparam name="TCommand">The type of command to handle.</typeparam>
    /// <typeparam name="TData">The type of returned data.</typeparam>
    public interface ICommandHandler<in TCommand, TData> : IOperationHandler<TCommand, TData>
        where TCommand : ICommand<TData>, new()
    {
    }
}
