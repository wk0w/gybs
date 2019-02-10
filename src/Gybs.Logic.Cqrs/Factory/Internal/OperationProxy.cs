using Gybs.Logic.Cqrs.Internal;

namespace Gybs.Logic.Cqrs.Factory.Internal
{
    internal class OperationProxy<TOperation> : IOperationProxy<TOperation>
        where TOperation : IOperation
    {
        public TOperation Operation { get; }
        public IOperationBus Bus { get; }

        public OperationProxy(TOperation operation, IOperationBus operationBus)
        {
            Operation = operation;
            Bus = operationBus;
        }
    }
}
