using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gybs.Results.Internal;

namespace Gybs.Results
{
    /// <summary>
    /// Creates results.
    /// </summary>
    public static class Result
    {
        /// <summary>
        /// Gets or sets the factory used to create the results. 
        /// </summary>
        /// <remarks>
        /// Defaults to internal implementation. 
        /// </remarks>
        public static IResultFactory Factory { get; set; } = new ResultFactory();

        /// <summary>
        /// Creates a copy of the result with specified data type and new data.
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="result">The result to copy.</param>
        /// <param name="data">The data to return.</param>
        /// <param name="errors">New errors; if null, source result's dictionary is used.</param>
        /// <param name="metadata">New metadata; if null, source result's dictionary is used.</param>
        /// <returns>The copy of the result.</returns>
        public static IResult<TData> Copy<TData>(IResult result, TData data,
            IReadOnlyDictionary<string, IReadOnlyCollection<string>>? errors = null,
            IReadOnlyDictionary<string, object>? metadata = null)
            => result.HasSucceeded
                ? Factory.CreateSuccess(data, metadata ?? result.Metadata)
                : Factory.CreateFailure<TData>(errors ?? result.Errors, metadata ?? result.Metadata);

        /// <summary>
        /// Creates the successful result.
        /// </summary>
        /// <typeparam name="TData">The type of returned data.</typeparam>
        /// <param name="data">The data to return.</param>
        /// <returns>The result.</returns>
        public static IResult<TData> Success<TData>(TData data) => Factory.CreateSuccess(data, default);

        /// <summary>
        /// Creates the successful result.
        /// </summary>
        /// <returns>The result.</returns>
        public static IResult Success() => Success<object?>(default);

        /// <summary>
        /// Creates the failed result.
        /// </summary>
        /// <param name="errors">The dictionary of errors.</param>
        /// <returns>The result.</returns>
        public static IResult Failure(IReadOnlyDictionary<string, IReadOnlyCollection<string>> errors) => Factory.CreateFailure(errors, default);

        /// <summary>
        /// Creates the failed result with a single error.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>The result.</returns>
        public static IResult Failure(string key, params string[] messages) => Failure(new ResultErrorsDictionary().Add(key, messages));

        /// <summary>
        /// Creates the failed result with a single error.
        /// </summary>
        /// <typeparam name="TType">Type used to build the key.</typeparam>
        /// <param name="propertyExpression">Expression used to generate the key.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>The result.</returns>
        public static IResult Failure<TType>(Expression<Func<TType, object>> propertyExpression, params string[] messages) => Failure(new ResultErrorsDictionary().Add(propertyExpression, messages));
    }
}
