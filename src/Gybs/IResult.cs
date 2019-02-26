using System.Collections.Generic;

namespace Gybs
{
    /// <summary>
    /// Represents a result of the operation.
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Gets whether operation has completed successfully.
        /// </summary>
        bool HasSucceeded { get; }

        /// <summary>
        /// Gets the errors dictionary.
        /// </summary>
        IReadOnlyDictionary<string, IReadOnlyCollection<string>> Errors { get; }

        /// <summary>
        /// Gets additional metadata fields.
        /// </summary>
        IReadOnlyDictionary<string, object> Metadata { get; }
    }

    /// <summary>
    /// Represents a result of the operation with returned data.
    /// </summary>
    /// <typeparam name="TData">The type of returned data.</typeparam>
    public interface IResult<out TData> : IResult
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        TData Data { get; }
    }
}
