using System.Collections.Generic;

namespace Gybs.Results.Internal;

internal class Result<TData> : IResult<TData>
{
    public bool HasSucceeded { get; }
    public TData? Data { get; }
    public IReadOnlyDictionary<string, IReadOnlyCollection<string>> Errors { get; set; } = new Dictionary<string, IReadOnlyCollection<string>>();
    public IReadOnlyDictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

    public Result(bool hasSucceeded, TData? data)
    {
        HasSucceeded = hasSucceeded;
        Data = data;
    }
}
