namespace Gybs.Logic.Operations;

/// <summary>
/// Represents a base interface for operation
/// </summary>
public interface IOperationBase
{
}

/// <summary>
/// Represents an operation which can be handled.
/// </summary>
public interface IOperation : IOperationBase
{
}

/// <summary>
/// Represents an operation which can be handled.
/// </summary>
public interface IOperation<TData> : IOperationBase
{
}
