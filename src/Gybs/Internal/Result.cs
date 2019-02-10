using System.Collections.Generic;

namespace Gybs.Internal
{
    internal class Result<TData> : IResult<TData>
    {
        public bool HasSucceeded { get; }
        public IReadOnlyList<ResultError> Errors { get; }
        public IReadOnlyDictionary<string, object> Metadata { get; }
        public TData Data { get; }

        public Result(bool hasSucceeded, IReadOnlyList<ResultError> errors, IReadOnlyDictionary<string, object> metadata, TData data)
        {
            HasSucceeded = hasSucceeded;
            Errors = errors ?? new ResultError[0];
            Metadata = metadata ?? new Dictionary<string, object>();
            Data = data;
        }        
    }
}
