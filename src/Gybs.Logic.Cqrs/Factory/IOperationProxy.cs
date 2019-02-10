using Gybs.Logic.Cqrs.Internal;

namespace Gybs.Logic.Cqrs.Factory
{
    /// <summary>
    /// Represents proxy for operation ready to handle.
    /// </summary>
    /// <typeparam name="TOperation">Type of operation.</typeparam>
    public interface IOperationProxy<out TOperation>
        where TOperation : IOperation
    {
        /// <summary>
        /// Gets operation.
        /// </summary>
        TOperation Operation { get; }

        /// <summary>
        /// Gets service which is able to handle operation.
        /// </summary>
        IOperationBus Bus { get; }
    }
}
