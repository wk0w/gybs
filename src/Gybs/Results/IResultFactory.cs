using System.Collections.Generic;

namespace Gybs.Results
{
    /// <summary>
    /// Represents a factory for thre results.
    /// </summary>
    public interface IResultFactory
    {
        /// <summary>
        /// Creates the successful result.
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="data">The data to return.</param>
        /// <param name="metadata">The additional metadata fields.</param>
        /// <returns>The result.</returns>
        IResult<TData> CreateSuccess<TData>(TData data, IReadOnlyDictionary<string, object> metadata);

        /// <summary>
        /// Creates the successful result.
        /// </summary>
        /// <param name="metadata">The additional metadata fields.</param>
        /// <returns>The result.</returns>
        IResult CreateSuccess(IReadOnlyDictionary<string, object> metadata);

        /// <summary>
        /// Creates the failed result.
        /// </summary>
        /// <param name="errors">The dictionary of errors.</param>
        /// <param name="metadata">The additional metadata fields.</param>
        /// <returns>The result.</returns>
        IResult CreateFailure(IReadOnlyDictionary<string, IReadOnlyCollection<string>> errors, IReadOnlyDictionary<string, object> metadata);
    }
}
