using Gybs.Extensions;
using Gybs.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gybs.Results
{
    internal class Result<TData> : IResult<TData>
    {
        public bool HasSucceeded { get; }
        public TData Data { get; }        
        public IReadOnlyDictionary<string, IReadOnlyCollection<string>> Errors { get; set; } = new Dictionary<string, IReadOnlyCollection<string>>();
        public IReadOnlyDictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
        
        public Result(bool hasSucceeded, TData data)
        {
            HasSucceeded = hasSucceeded;
            Data = data;
        }        
    }

    /// <summary>
    /// Creates results.
    /// </summary>
    public static class Result
    {        
        /// <summary>
        /// Creates a copy of the result with specified data type and new data.
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="result">The result to copy.</param>
        /// <param name="data">The data to return.</param>
        /// <param name="errors">New errors; if null, source result's dictionary is used.</param>
        /// <param name="metadata">New metadata; if null, source result's dictionary is used.</param>
        /// <returns>The copy of the result.</returns>
        public static IResult<TData> Copy<TData>(IResult result, TData data, IReadOnlyDictionary<string, IReadOnlyCollection<string>> errors = null, IReadOnlyDictionary<string, object> metadata = null) 
            => new Result<TData>(result.HasSucceeded, data) { Errors = errors ?? result.Errors, Metadata = metadata ?? result.Metadata };

        /// <summary>
        /// Creates the successful result.
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="data">The data to return.</param>
        /// <returns>The result.</returns>
        public static IResult<TData> Success<TData>(TData data) => new Result<TData>(true, data);

        /// <summary>
        /// Creates the successful result.
        /// </summary>
        /// <returns>The result.</returns>
        public static IResult Success() => Success<object>(null);

        /// <summary>
        /// Creates the failed result.
        /// </summary>
        /// <param name="errors">The dictionary of errors.</param>
        /// <returns>The result.</returns>
        public static IResult Failure(IReadOnlyDictionary<string, IReadOnlyCollection<string>> errors) => new Result<object>(false, default) { Errors = errors };
    }
}
