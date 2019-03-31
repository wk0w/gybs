namespace Gybs.Logic.Operations.Factory
{
    /// <summary>
    /// Represents a proxy for operation ready to handle.
    /// </summary>
    /// <typeparam name="TOperation">Type of operation.</typeparam>
    public interface IOperationProxy<out TOperation>
        where TOperation: IOperationBase
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
