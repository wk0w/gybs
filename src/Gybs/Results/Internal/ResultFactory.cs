using System.Collections.Generic;

namespace Gybs.Results.Internal
{
    internal class ResultFactory : IResultFactory
    {
        public IResult<TData> CreateSuccess<TData>(TData data, IReadOnlyDictionary<string, object>? metadata)
        {
            var result = new Result<TData>(true, data);

            if (metadata is { })
            {
                result.Metadata = metadata;
            }

            return result;
        }

        public IResult CreateSuccess(IReadOnlyDictionary<string, object>? metadata)
            => CreateSuccess<object?>(default, metadata);
        
        public IResult<TData> CreateFailure<TData>(IReadOnlyDictionary<string, IReadOnlyCollection<string>> errors, IReadOnlyDictionary<string, object>? metadata)
        {
            var result = new Result<TData>(false, default) { Errors = errors };
            
            if (metadata is { })
            {
                result.Metadata = metadata;
            }

            return result;
        }

        public IResult CreateFailure(IReadOnlyDictionary<string, IReadOnlyCollection<string>> errors, IReadOnlyDictionary<string, object>? metadata)
            => CreateFailure<object?>(errors, metadata);
    }
}
