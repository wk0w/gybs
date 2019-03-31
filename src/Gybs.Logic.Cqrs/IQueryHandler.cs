using System.Threading.Tasks;
using Gybs.Logic.Operations;

namespace Gybs.Logic.Cqrs
{   
    /// <summary>
    /// Represents a handler of a query.
    /// </summary>
    /// <typeparam name="TQuery">The type of query to handle.</typeparam>
    /// <typeparam name="TData">The type of returned data.</typeparam>
    public interface IQueryHandler<in TQuery, TData> : IOperationHandler<TQuery, TData>
        where TQuery : IQuery<TData>, new()
    {
    }
}
