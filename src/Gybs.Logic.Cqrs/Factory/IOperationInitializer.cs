using Gybs.Logic.Cqrs.Internal;

namespace Gybs.Logic.Cqrs.Factory
{
    /// <summary>
    /// Represents shared initialization logic for operations.
    /// </summary>
    public interface IOperationInitializer
    {
        /// <summary>
        /// Initializes operation.
        /// </summary>
        /// <param name="operation">Operation.</param>
        void Initialize(IOperation operation);
    }
}
