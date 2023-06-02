namespace Gybs.Logic.Operations.Factory.Internal;

internal class OperationProxy<TOperation> : IOperationProxy<TOperation>
    where TOperation : IOperationBase
{
    public TOperation Operation { get; }
    public IOperationBus Bus { get; }

    public OperationProxy(TOperation operation, IOperationBus operationBus)
    {
        Operation = operation;
        Bus = operationBus;
    }
}
