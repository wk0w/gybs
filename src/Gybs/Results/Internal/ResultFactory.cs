using System.Collections.Generic;

namespace Gybs.Results.Internal;

internal class ResultFactory : IResultFactory
{
    public IResult<TData> CreateSuccess<TData>(TData data, IReadOnlyDictionary<string, object>? metadata)
    {
        var result = new Result<TData>(true, data);

        if (metadata is not null)
        {
            result.Metadata = metadata;
        }

        return result;
    }

    public IResult CreateSuccess(IReadOnlyDictionary<string, object>? metadata)
    {
        return CreateSuccess<object?>(default, metadata);
    }

    public IResult<TData> CreateFailure<TData>(IReadOnlyDictionary<string, IReadOnlyCollection<string>> errors, IReadOnlyDictionary<string, object>? metadata)
    {
        var result = new Result<TData>(false, default) { Errors = errors };

        if (metadata is not null)
        {
            result.Metadata = metadata;
        }

        return result;
    }

    public IResult CreateFailure(IReadOnlyDictionary<string, IReadOnlyCollection<string>> errors, IReadOnlyDictionary<string, object>? metadata)
    {
        return CreateFailure<object?>(errors, metadata);
    }
}
