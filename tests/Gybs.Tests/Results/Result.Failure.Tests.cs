using FluentAssertions;
using Gybs.Results;
using System.Collections.Generic;
using Xunit;

namespace Gybs.Tests.Results
{
    public class ResultFailureTests
    {
        [Fact]
        public void ForErrorsShouldBeFailed()
        {
            var result = Result.Failure(new Dictionary<string, IReadOnlyCollection<string>>());
            result.HasSucceeded.Should().BeFalse();
        }

        [Fact]
        public void ForErrorsShouldSetErrors()
        {
            var errors = new Dictionary<string, IReadOnlyCollection<string>> { ["key"] = new List<string> { string.Empty } };
            var result = Result.Failure(errors);
            result.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public void ForSingleStringErrorShouldSetError()
        {
            var errors = new Dictionary<string, IReadOnlyCollection<string>> { ["key"] = new List<string> { "value" } };
            var result = Result.Failure("key", "value");
            result.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public void ForSingleModelErrorShouldSetError()
        {
            var errors = new Dictionary<string, IReadOnlyCollection<string>> { ["Model.Test"] = new List<string> { "value" } };
            var result = Result.Failure<Model>(m => m.Test, "value");
            result.Errors.Should().BeEquivalentTo(errors);
        }

        private class Model
        {
            public string Test { get; set; } = string.Empty;
        }
    }
}
