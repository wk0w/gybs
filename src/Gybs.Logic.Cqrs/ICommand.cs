using Gybs.Logic.Operations;

namespace Gybs.Logic.Cqrs;

/// <summary>
/// Represents a command to handle.
/// </summary>
public interface ICommand : IOperation
{
}

/// <summary>
/// Represents a command to handle.
/// </summary>
/// <typeparam name="TData">The type of returned data.</typeparam>
public interface ICommand<TData> : IOperation<TData>
{
}
