using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gybs.Extensions;
using Gybs.Internal;

namespace Gybs
{
    /// <summary>
    /// Represents a result of the operation.
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Gets whether operation has completed successfuly.
        /// </summary>
        bool HasSucceeded { get; }

        /// <summary>
        /// Gets the list of errors.
        /// </summary>
        IReadOnlyList<ResultError> Errors { get; }

        /// <summary>
        /// Gets additional metadata fields.
        /// </summary>
        IReadOnlyDictionary<string, object> Metadata { get; }
    }

    /// <summary>
    /// Represents a result of the operation with returned data.
    /// </summary>
    /// <typeparam name="TData">The type of returned data..</typeparam>
    public interface IResult<out TData> : IResult
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        TData Data { get; }
    }

    /// <summary>
    /// Creates results.
    /// </summary>
    public static class Result
    {        
        /// <summary>
        /// Creates the successful result.
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="data">The data to return.</param>
        /// <param name="metadata">Additional metadata.</param>
        /// <returns>The result.</returns>
        public static IResult<TData> Success<TData>(TData data, IReadOnlyDictionary<string, object> metadata = null) => new Result<TData>(true, null, metadata, data);

        /// <summary>
        /// Creates the successful result.
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="data">The data to return.</param>
        /// <param name="metadata">Additional metadata.</param>
        /// <returns>The result.</returns>
        public static Task<IResult<TData>> SuccessAsync<TData>(TData data, IReadOnlyDictionary<string, object> metadata = null) => Success(data).ToCompletedTask();

        /// <summary>
        /// Creates the successful result.
        /// </summary>
        /// <param name="metadata">Additional metadata.</param>
        /// <returns>The result.</returns>
        public static IResult Success(IReadOnlyDictionary<string, object> metadata = null) => Success<object>(null, metadata);

        /// <summary>
        /// Creates the successful result.
        /// </summary>
        /// <param name="metadata">Additional metadata.</param>
        /// <returns>The result.</returns>
        public static Task<IResult> SuccessAsync(IReadOnlyDictionary<string, object> metadata = null) => Success(metadata).ToCompletedTask();

        /// <summary>
        /// Creates the failed result.
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="errors">The list of errors.</param>
        /// <param name="metadata">Additional metadata.</param>
        /// <returns>The result.</returns>
        public static IResult<TData> Failure<TData>(IReadOnlyList<ResultError> errors, IReadOnlyDictionary<string, object> metadata = null) => new Result<TData>(false, errors, metadata, default);

        /// <summary>
        /// Creates the failed result.
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="errors">The list of errors.</param>
        /// <param name="metadata">Additional metadata.</param>
        /// <returns>The result.</returns>
        public static Task<IResult<TData>> FailureAsync<TData>(IReadOnlyList<ResultError> errors, IReadOnlyDictionary<string, object> metadata = null) => Failure<TData>(errors, metadata).ToCompletedTask();

        /// <summary>
        /// Creates the failed result.
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="error">The error.</param>
        /// <param name="metadata">Additional metadata.</param>
        /// <returns>The result.</returns>
        public static IResult<TData> Failure<TData>(ResultError error, IReadOnlyDictionary<string, object> metadata = null) => Failure<TData>(new[] { error }, metadata);

        /// <summary>
        /// Creates the failed result.
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="error">The error.</param>
        /// <param name="metadata">Additional metadata.</param>
        /// <returns>The result.</returns>
        public static Task<IResult<TData>> FailureAsync<TData>(ResultError error, IReadOnlyDictionary<string, object> metadata = null) => Failure<TData>(error, metadata).ToCompletedTask();

        /// <summary>
        /// Creates the failed result.
        /// </summary>
        /// <param name="errors">The list of errors.</param>
        /// <param name="metadata">Additional metadata.</param>
        /// <returns>The result.</returns>
        public static IResult Failure(IReadOnlyList<ResultError> errors, IReadOnlyDictionary<string, object> metadata = null) => Failure<object>(errors, metadata);

        /// <summary>
        /// Creates the failed result.
        /// </summary>
        /// <param name="errors">The list of errors.</param>
        /// <param name="metadata">Additional metadata.</param>
        /// <returns>The result.</returns>
        public static Task<IResult> FailureAsync(IReadOnlyList<ResultError> errors, IReadOnlyDictionary<string, object> metadata = null) => Failure(errors, metadata).ToCompletedTask();
        
        /// <summary>
        /// Creates the failed result.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="metadata">Additional metadata.</param>
        /// <returns>The result.</returns>
        public static IResult Failure(ResultError error, IReadOnlyDictionary<string, object> metadata = null) => Failure<object>(new [] { error }, metadata);

        /// <summary>
        /// Creates the failed result.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="metadata">Additional metadata.</param>
        /// <returns>The result.</returns>
        public static Task<IResult> FailureAsync(ResultError error, IReadOnlyDictionary<string, object> metadata = null) => Failure(error, metadata).ToCompletedTask();

        /// <summary>
        /// Creates copy of the result with specified data type.
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="result">The Result copy.</param>
        /// <param name="data">The data to return.</param>
        /// <returns>The copy of the result.</returns>
        public static IResult<TData> Copy<TData>(IResult result, TData data = default) => new Result<TData>(result.HasSucceeded, result.Errors, result.Metadata, data);
    }

    /// <summary>
    /// Represents an error for the key.
    /// </summary>
    public sealed class ResultError
    {
        /// <summary>
        /// Gets the key of the error.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the messages associated with the key.
        /// </summary>
        public string[] Messages { get; }

        private ResultError(
            string key,
            params string[] messages)
        {
            Key = key;
            Messages = messages;
        }

        /// <summary>
        /// Creates new instance of the result error.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="messages">The messages.</param>
        public static ResultError Create(string key, params string[] messages) => new ResultError(key, messages);

        /// <summary>
        /// Creates new instance of the result error with the key generated from the expression (with pattern Type.PropertyA.PropertyB).
        /// </summary>
        /// <typeparam name="TType">Type used to build the key.</typeparam>
        /// <param name="propertyExpression">Expression used to generate the key.</param>
        /// <param name="messages">The messages.</param>
        public static ResultError Create<TType>(Expression<Func<TType, object>> propertyExpression, params string[] messages)
        {
            var memberExpression = propertyExpression.Body as MemberExpression
                                   ?? (propertyExpression.Body as UnaryExpression)?.Operand as MemberExpression;

            if (memberExpression == null)
            {
                throw new ArgumentException("Provided expression is not valid.", nameof(propertyExpression));
            }

            var names = new Stack<string>();

            while (memberExpression != null)
            {
                names.Push(memberExpression.Member.Name);
                memberExpression = memberExpression.Expression as MemberExpression;
            }

            var key = $"{typeof(TType).Name}.{string.Join(".", names)}";
            return new ResultError(key, messages);
        }
    }

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
        /// <param name="metadata">Additional metadata.</param>
        /// <returns>The result.</returns>
        public static IResult<TData> ToSuccessfulResult<TData>(this TData data, IReadOnlyDictionary<string, object> metadata = null)
        {
            return Result.Success(data);
        }

        /// <summary>
        /// Creates the successful result.
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="data">The data to return.</param>
        /// <param name="metadata">Additional metadata.</param>
        /// <returns>The result.</returns>
        public static Task<IResult<TData>> ToSuccessfulResultAsync<TData>(this TData data, IReadOnlyDictionary<string, object> metadata = null)
        {
            return Result.SuccessAsync(data);
        }

        /// <summary>
        /// Creates the successful result with pagination metadata (offset and limit).
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="data">The data to return.</param>
        /// <param name="offset">The offset of the paginated data.</param>
        /// <param name="limit">The limit of the paginated data.</param>
        /// <returns>The result.</returns>
        public static IResult<TData> ToSuccessfulPagedResult<TData>(this TData data, int offset, int limit)
        {
            return data.ToSuccessfulResult(new Dictionary<string, object>
            {
                { nameof(offset), offset },
                { nameof(limit), limit }
            });
        }

        /// <summary>
        /// Creates the successful result.
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="data">The data to return.</param>
        /// <param name="offset">The offset of the paginated data.</param>
        /// <param name="limit">The limit of the paginated data.</param>
        /// <returns>The result.</returns>
        public static Task<IResult<TData>> ToSuccessfulPagedResultAsync<TData>(this TData data, int offset, int limit)
        {
            return data.ToSuccessfulResultAsync(new Dictionary<string, object>
            {
                { nameof(offset), offset },
                { nameof(limit), limit }
            });
        }

        /// <summary>
        /// Flattens the errors from multiple results into a single one.
        /// </summary>
        /// <param name="results">The results to flatten.</param>
        /// <returns>The result.</returns>
        public static IResult Flatten(this IEnumerable<IResult> results)
        {
            results = results.ToList();

            var hasSucceeded = results.All(r => r.HasSucceeded);
            var metadata = results
                .SelectMany(r => r.Metadata)
                .GroupBy(k => k.Key)
                .ToDictionary(k => k.Key, v => v.First().Value);
            var errors = results
                .SelectMany(r => r.Errors)
                .GroupBy(r => r.Key)
                .Select(g => ResultError.Create(g.Key, g.SelectMany(r => r.Messages).ToArray()))
                .ToList();

            return new Result<object>(hasSucceeded, errors, metadata, default);
        }

        /// <summary>
        /// Aggregates errors of multiple results into single one.
        /// </summary>
        /// <param name="results">Results.</param>
        /// <returns>Single result.</returns>
        public static IResult<TData> Flatten<TData>(this IEnumerable<IResult<TData>> results)
        {
            var baseResults = (IEnumerable<IResult>)results;
            return Result.Copy<TData>(baseResults.Flatten());
        }

        /// <summary>
        /// Maps the result into a new one.
        /// </summary>
        /// <typeparam name="TOutputData">The type of output data.</typeparam>
        /// <param name="inputResult">The result to map.</param>
        /// <returns>Mapped result.</returns>
        public static IResult<TOutputData> Map<TOutputData>(this IResult inputResult)
        {
            return Result.Copy<TOutputData>(inputResult);
        }

        /// <summary>
        /// Maps the result into a new one.
        /// </summary>
        /// <typeparam name="TInputData">The type of input data.</typeparam>
        /// <typeparam name="TOutputData">The type of output data.</typeparam>
        /// <param name="inputResult">The result to map.</param>
        /// <param name="mapper">Map function invoked on the data.</param>
        /// <returns>Mapped result.</returns>
        public static IResult<TOutputData> Map<TInputData, TOutputData>(this IResult<TInputData> inputResult, Func<TInputData, TOutputData> mapper)
        {
            return Result.Copy(inputResult, mapper.Invoke(inputResult.Data));
        }
    }    
}
