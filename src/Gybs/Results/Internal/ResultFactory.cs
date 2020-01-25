using System.Collections.Generic;

namespace Gybs.Results.Internal
{
    internal class ResultFactory : IResultFactory
    {
        public IResult<TData> CreateSuccess<TData>(TData data, IReadOnlyDictionary<string, object>? metadata)
        {
            var result = Result.Success(data);

            if (metadata is { })
            {
                result = result.AddMetadata(metadata);
            }

            return result;
        }

        public IResult CreateSuccess(IReadOnlyDictionary<string, object>? metadata)
        {
            var result = Result.Success();

            if (metadata is { })
            {
                result = result.AddMetadata(metadata);
            }

            return result;
        }

        public IResult CreateFailure(IReadOnlyDictionary<string, IReadOnlyCollection<string>> errors, IReadOnlyDictionary<string, object>? metadata)
        {
            var result = Result.Failure(errors);

            if (metadata is { })
            {
                result = result.AddMetadata(metadata);
            }

            return result;
        }
    }
}
