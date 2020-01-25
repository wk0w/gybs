using FluentAssertions;
using Gybs;
using Gybs.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Gybs.Tests.Results
{
    public class ResultCopyTests
    {
        [Theory, InlineData(false), InlineData(true)]
        public void ForResultShouldCopyResult(bool hasSucceeded)
        {
            var result = new Result<object?>(hasSucceeded, null);
            var copy = Result.Copy<int>(result, default);
            copy.HasSucceeded.Should().Be(hasSucceeded);
        }

        [Fact]
        public void ForDataShouldSetNewData()
        {
            var result = new Result<object?>(true, null);
            var copy = Result.Copy(result, 2);
            copy.Data.Should().Be(2);
        }

        [Fact]
        public void ForNoErrorsShouldSetSourceCollection()
        {
            var errors = new Dictionary<string, IReadOnlyCollection<string>> { ["key"] = new List<string> { string.Empty } };
            var result = new Result<object?>(false, null) { Errors = errors };
            var copy = Result.Copy(result, 2);
            copy.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public void ForErrorsShouldSetNewErrors()
        {
            var newErrors = new Dictionary<string, IReadOnlyCollection<string>> { ["key"] = new List<string> { string.Empty } };
            var result = new Result<object?>(false, null) { Errors = new Dictionary<string, IReadOnlyCollection<string>> { ["somediffrentkey"] = new List<string> { string.Empty } } };
            var copy = Result.Copy(result, 2, newErrors);
            copy.Errors.Should().BeEquivalentTo(newErrors);
        }

        [Fact]
        public void ForNoMetadataShouldSetSourceDictionary()
        {
            var metadata = new Dictionary<string, object> { ["t"] = 5 };
            var result = new Result<object?>(false, null) { Metadata = metadata };
            var copy = Result.Copy(result, 2);
            copy.Metadata.Should().BeEquivalentTo(metadata);
        }

        [Fact]
        public void ForErrorsShouldSetNewMetadata()
        {
            var newMetadata = new Dictionary<string, object> { ["t"] = 5 };
            var result = new Result<object?>(false, null) { Metadata = new Dictionary<string, object> { ["t"] = 2 } };
            var copy = Result.Copy(result, 2, metadata: newMetadata);
            copy.Metadata.Should().BeEquivalentTo(newMetadata);
        }
    }
}
