using FluentAssertions;
using Gybs;
using Gybs.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Gybs.Tests.Results
{
    public class ResultSuccessTests
    {
        [Fact]
        public void ForSuccessShouldBeSuccessful()
        {
            var result = Result.Success();
            result.HasSucceeded.Should().BeTrue();
        }

        [Fact]
        public void ForDataShouldBeSuccessful()
        {
            var result = Result.Success(17);
            result.HasSucceeded.Should().BeTrue();
        }

        [Fact]
        public void ForDataShouldSetData()
        {
            const int data = 17;
            var result = Result.Success(data);
            result.Data.Should().Be(data);
        }
    }
}
