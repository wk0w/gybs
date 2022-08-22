using System;

namespace Gybs.Logic.Validation;

/// <summary>
/// Represents a exception thrown when validation failed.
/// </summary>
public class ValidationFailedException : Exception
{
    /// <summary>
    /// Gets the result.
    /// </summary>
    public IResult Result { get; }

    /// <summary>
    /// Creates new instance of the exception.
    /// </summary>
    /// <param name="result">Validation result.</param>
    public ValidationFailedException(IResult result)
        : base("Validation failed.")
    {
        Result = result;
    }
}
