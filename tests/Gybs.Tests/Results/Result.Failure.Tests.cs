using FluentAssertions;
using Gybs;
using Gybs.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    }
}
