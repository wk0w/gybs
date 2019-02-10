using Gybs.Logic.Cqrs.Internal;

namespace Gybs.Logic.Cqrs
{
    /// <summary>
    /// Represents a query to handle.
    /// </summary>
    /// <typeparam name="TData">The type of returned data.</typeparam>
    public interface IQuery<TData> : IOperation<TData>
    {
    }
}
