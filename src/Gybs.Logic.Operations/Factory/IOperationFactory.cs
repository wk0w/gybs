using System;

namespace Gybs.Logic.Operations.Factory;

/// <summary>
/// Represents factory for creating and handling operations.
/// </summary>
public interface IOperationFactory
{
    /// <summary>
    /// Creates and initializes new operation.
    /// </summary>
    /// <typeparam name="TOperation">Type implementing <see cref="IOperationBase"/>.</typeparam>
    /// <returns>Operation to handle.</returns>
    IOperationProxy<TOperation> Create<TOperation>()
        where TOperation : IOperationBase, new();

    /// <summary>
    /// Creates and initializes new operation.
    /// </summary>
    /// <typeparam name="TOperation">Type implementing <see cref="IOperationBase"/>.</typeparam>
    /// <param name="initializer">Action which initializes the operation. Invoked after <see cref="IOperationInitializer"/> implementations.</param>
    /// <returns>Operation to handle.</returns>
    IOperationProxy<TOperation> Create<TOperation>(Action<TOperation> initializer)
        where TOperation : IOperationBase, new();

    /// <summary>
    /// Uses existing operation and initializes it.
    /// </summary>
    /// <param name="operation">Operation.</param>
    /// <typeparam name="TOperation">Type implementing <see cref="IOperationBase"/>.</typeparam>
    /// <returns>Operation to handle.</returns>
    IOperationProxy<TOperation> UseExisting<TOperation>(TOperation operation)
        where TOperation : IOperationBase, new();

    /// <summary>
    /// Uses existing operation and initializes it.
    /// </summary>
    /// <param name="operation">Operation.</param>
    /// <param name="initializer">Action which initializes the operation. Invoked after <see cref="IOperationInitializer"/> implementations.</param>
    /// <typeparam name="TOperation">Type implementing <see cref="IOperationBase"/>.</typeparam>
    /// <returns>Operation to handle.</returns>
    IOperationProxy<TOperation> UseExisting<TOperation>(TOperation operation, Action<TOperation> initializer)
        where TOperation : IOperationBase, new();

    /// <summary>
    /// Uses existing operation and initializes it.
    /// </summary>
    /// <param name="operation">Operation.</param>
    /// <param name="factory">Factory method which which creates a new operation using an existing one. Invoked after <see cref="IOperationInitializer"/> implementations.</param>
    /// <typeparam name="TOperation">Type implementing <see cref="IOperationBase"/>.</typeparam>
    /// <returns>Operation to handle.</returns>
    IOperationProxy<TOperation> UseExisting<TOperation>(TOperation operation, Func<TOperation, TOperation> factory)
        where TOperation : IOperationBase, new();
}
