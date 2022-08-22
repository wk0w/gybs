﻿namespace Gybs.Internal;

/// <summary>
/// This interface is explicitly implemented by type to hide properties that are
/// not intended to be used in application code but can be used in extension methods
/// written by database providers etc.
/// It is generally not used in application code.
/// </summary>
/// <typeparam name="T">The type of the property being hidden.</typeparam>
public interface IInfrastructure<out T>
{
    /// <summary>
    /// Gets the value of the property being hidden.
    /// </summary>
    T Instance { get; }
}
