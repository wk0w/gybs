using System;
using System.Collections.Generic;

namespace Gybs.Results
{
    /// <summary>
    /// <see cref="IResult"/> extensions.
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Creates the successful result.
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="data">The data to return.</param>
        /// <returns>The result.</returns>
        public static IResult<TData> ToSuccessfulResult<TData>(this TData data) => Result.Success(data);

        /// <summary>
        /// Maps the result into a new one.
        /// </summary>
        /// <typeparam name="TOutputData">The type of output data.</typeparam>
        /// <param name="inputResult">The result to map.</param>
        /// <returns>Mapped result.</returns>
        public static IResult<TOutputData> Map<TOutputData>(this IResult inputResult) => Result.Copy<TOutputData>(inputResult, default);

        /// <summary>
        /// Maps the result into a new one.
        /// </summary>
        /// <typeparam name="TInputData">The type of input data.</typeparam>
        /// <typeparam name="TOutputData">The type of output data.</typeparam>
        /// <param name="inputResult">The result to map.</param>
        /// <param name="mapper">Map function invoked on the data.</param>
        /// <returns>Mapped result.</returns>
        public static IResult<TOutputData> Map<TInputData, TOutputData>(this IResult<TInputData> inputResult, Func<TInputData, TOutputData> mapper)
            => Result.Copy(inputResult, mapper.Invoke(inputResult.Data));

        /// <summary>
        /// Adds the metadata and returns the new result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="metadata">The metadata.</param>
        /// <typeparam name="TData">The type of data.</typeparam>
        /// <returns>The result</returns>
        public static IResult<TData> AddMetadata<TData>(this IResult<TData> result, IReadOnlyDictionary<string, object> metadata)
            => Result.Copy(result, result.Data, metadata: metadata);

        /// <summary>
        /// Adds the metadata and returns the new result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The result</returns>
        public static IResult AddMetadata(this IResult result, IReadOnlyDictionary<string, object> metadata)
            => Result.Copy<object>(result, default, metadata: metadata);
    }
}
