using System;
using Gybs.Logic.Cqrs.Internal;

namespace Gybs.Logic.Cqrs.Factory
{
    /// <summary>
    /// Represents factory for creating and handling operations.
    /// </summary>
    public interface IOperationFactory
    {
        /// <summary>
        /// Creates operation.
        /// </summary>
        /// <typeparam name="TOperation">Type implementing <see cref="IOperation"/>.</typeparam>
        /// <returns>Operation to handle.</returns>
        IOperationProxy<TOperation> Create<TOperation>()
            where TOperation : IOperation, new();

        /// <summary>
        /// Creates operation.
        /// </summary>
        /// <typeparam name="TOperation">Type implementing <see cref="IOperation"/>.</typeparam>
        /// <param name="initializer">Action which initializes operation. Invoked after <see cref="IOperationInitializer"/> implementations.</param>
        /// <returns>Operation to handle.</returns>
        IOperationProxy<TOperation> Create<TOperation>(Action<TOperation> initializer)
            where TOperation : IOperation, new();
    }
}
