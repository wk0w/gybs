using System.Collections.Generic;

namespace Gybs.Results.Internal
{
    internal class ResultFactory : IResultFactory
    {
        public IResult<TData> CreateSuccess<TData>(TData data, IReadOnlyDictionary<string, object> metadata) => Result.Success(data).AddMetadata(metadata);

        public IResult CreateSuccess(IReadOnlyDictionary<string, object> metadata) => Result.Success().AddMetadata(metadata);

        public IResult CreateFailure(IReadOnlyDictionary<string, IReadOnlyCollection<string>> errors, IReadOnlyDictionary<string, object> metadata) =>
            Result.Failure(errors).AddMetadata(metadata);
    }
}
