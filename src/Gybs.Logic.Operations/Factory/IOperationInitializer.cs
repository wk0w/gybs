using System.Threading.Tasks;

namespace Gybs.Logic.Operations.Factory
{
    /// <summary>
    /// Represents a shared initialization logic for operations.
    /// </summary>
    public interface IOperationInitializer
    {
        /// <summary>
        /// Initializes the operation.
        /// </summary>
        /// <param name="operation">The operation to initialize.</param>
        void Initialize(IOperationBase operation);
    }
}
