namespace Gybs.Logic.Operations.Factory;

/// <summary>
/// Represents a shared initialization logic for immutable operations.
/// </summary>
public interface IImmutableOperationInitializer
{
    /// <summary>
    /// Creates a new operation using an existing one.
    /// </summary>
    /// <param name="operation">The operation to be used during the initialization.</param>
    IOperationBase Initialize(IOperationBase operation);
}
