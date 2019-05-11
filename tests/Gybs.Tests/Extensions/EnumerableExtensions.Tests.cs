using System.Threading.Tasks;
using FluentAssertions;
using Gybs.Extensions;
using Xunit;

namespace Gybs.Tests.Extensions
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void ForEnumerableShouldInvokeAction()
        {
            var enumerable = new[] { 1, 2, 3, 4 };
            var sum = 0;
            enumerable.ForEach(v => sum += v);
            sum.Should().Be(10);
        }

        [Fact]
        public async Task ForEnumerableShouldInvokeAsyncAction()
        {
            var enumerable = new[] { 1, 2, 3, 4 };
            var sum = 0;
            await enumerable.ForEachAsync(async v => await Task.Run(() => sum += v));
            sum.Should().Be(10);
        }
    }
}
