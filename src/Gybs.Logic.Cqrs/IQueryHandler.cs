using System.Threading.Tasks;

namespace Gybs.Logic.Cqrs
{   
    /// <summary>
    /// Represents a handler of a query.
    /// </summary>
    /// <typeparam name="TQuery">The type of query to handle.</typeparam>
    /// <typeparam name="TData">The type of returned data.</typeparam>
    public interface IQueryHandler<in TQuery, TData>
        where TQuery : IQuery<TData>, new()
    {        
        /// <summary>
        /// Handles the query.
        /// </summary>
        /// <param name="query">The query to handle.</param>
        /// <returns>The result with data.</returns>
        Task<IResult<TData>> HandleAsync(TQuery query);
    }
}
