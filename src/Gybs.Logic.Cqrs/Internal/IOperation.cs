namespace Gybs.Logic.Cqrs.Internal
{
    /// <summary>
    /// Represents an operation which can be handled.
    /// </summary>
    public interface IOperation
    {
    }

    /// <summary>
    /// Represents an operation which can be handled.
    /// </summary>
    public interface IOperation<TData> : IOperation
    {
    }
}
