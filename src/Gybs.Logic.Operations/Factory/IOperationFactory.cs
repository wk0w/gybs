using System;

namespace Gybs.Logic.Operations.Factory
{
    /// <summary>
    /// Represents factory for creating and handling operations.
    /// </summary>
    public interface IOperationFactory
    {
        /// <summary>
        /// Creates the operation.
        /// </summary>
        /// <typeparam name="TOperation">Type implementing <see cref="IOperationBase"/>.</typeparam>
        /// <returns>Operation to handle.</returns>
        IOperationProxy<TOperation> Create<TOperation>()
            where TOperation: IOperationBase, new();

        /// <summary>
        /// Creates operation.
        /// </summary>
        /// <typeparam name="TOperation">Type implementing <see cref="IOperationBase"/>.</typeparam>
        /// <param name="initializer">Action which initializes operation. Invoked after <see cref="IOperationInitializer"/> implementations.</param>
        /// <returns>Operation to handle.</returns>
        IOperationProxy<TOperation> Create<TOperation>(Action<TOperation> initializer)
            where TOperation: IOperationBase, new();
    }
}
