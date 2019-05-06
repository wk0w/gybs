using FluentAssertions;
using Gybs;
using Gybs.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Gybs.Tests.Results
{
    public class ResultErrorsDictionaryTests
    {
        [Fact]
        public void ForNewStringKeyShouldAddNewError()
        {
            var errors = new ResultErrorsDictionary
            {
                { "key", "value" }
            };
            errors.Should().BeEquivalentTo(new Dictionary<string, string[]> { ["key"] = new[] { "value" } });
        }

        [Fact]
        public void ForAlreadyExistingStringKeyShouldAddNewErrorMessage()
        {
            var errors = new ResultErrorsDictionary
            {
                { "key", "value" },
                { "key", "value2" }
            };
            errors.Should().BeEquivalentTo(new Dictionary<string, string[]> { ["key"] = new[] { "value", "value2" } });
        }

        [Fact]
        public void ForKeyFromClassPropertyShouldAddNewErrorWithProperName()
        {
            var errors = new ResultErrorsDictionary();
            errors.Add<Model>(m => m.PropertyA, "value");
            errors.Should().BeEquivalentTo(new Dictionary<string, string[]> { [$"Model.{nameof(Model.PropertyA)}"] = new[] { "value" } });
        }

        [Fact]
        public void ForKeyFromNestedClassMixedWithStructsPropertyShouldAddNewErrorWithProperName()
        {
            var errors = new ResultErrorsDictionary();
            errors.Add<Model>(m => m.PropertyB.PropertyC.PropertyD, "value");
            errors.Should().BeEquivalentTo(new Dictionary<string, string[]> { [$"Model.{nameof(Model.PropertyB)}.{nameof(Model.PropertyB.PropertyC)}.{nameof(Model.PropertyB.PropertyC.PropertyD)}"] = new[] { "value" } });
        }

        [Fact]
        public void ForKeyInDictionaryIndexerShouldReturnValue()
        {
            var errors = new ResultErrorsDictionary { { "key", "value" } };
            errors["key"].Should().BeEquivalentTo("value");
        }

        [Fact]
        public void ForKeyInDictionaryShouldHaveCount()
        {
            var errors = new ResultErrorsDictionary { { "key", "value" } };
            errors.Count.Should().Be(1);
        }

        [Fact]
        public void ForKeyInDictionaryContainsKeyShouldReturnTrue()
        {
            var errors = new ResultErrorsDictionary { { "key", "value" } };
            errors.ContainsKey("key").Should().BeTrue();
        }

        [Fact]
        public void ForNoKeyInDictionaryContainsKeyShouldReturnFalse()
        {
            var errors = new ResultErrorsDictionary();
            errors.ContainsKey("key").Should().BeFalse();
        }

        [Fact]
        public void ForKeyInDictionaryTryGetShouldReturnValue()
        {
            var errors = new ResultErrorsDictionary { { "key", "value" } };
            var result = errors.TryGetValue("key", out var value);
            new { result, value }.Should().BeEquivalentTo(new { result = true, value = new List<string> { "value" } });
        }

        [Fact]
        public void ForNoKeyInDictionaryTryGetShouldNotReturnValue()
        {
            var errors = new ResultErrorsDictionary();
            var result = errors.TryGetValue("key", out var value);
            new { result, value }.Should().BeEquivalentTo(new { result = false, value = (IReadOnlyCollection<string>)null });
        }

        [Fact]
        public void ForDictionaryWithElementsShouldIterate()
        {
            var keys = new[] { "key1", "key2" };
            var values = new[] { "value1", "value2" };

            var errors = new ResultErrorsDictionary
            {
                { keys[0], values[0] },
                { keys[1], values[1] }
            };

            var iteratedKeys = new List<string>();
            var iteratedValues = new List<string>();

            foreach (var error in errors)
            {
                iteratedKeys.Add(error.Key);
                iteratedValues.Add(error.Value.First());
            }

            iteratedKeys.Sort();
            iteratedValues.Sort();

            iteratedKeys.Should().BeEquivalentTo(keys);
            iteratedValues.Should().BeEquivalentTo(values);
        }

        internal class Model
        {
            public int PropertyA { get; set; }

            public InnerModel PropertyB { get; set; }

            public class InnerModel
            {
                public InnerModelInnerStruct PropertyC { get; set; }

                public struct InnerModelInnerStruct
                {
                    public string PropertyD { get; set; }
                }
            }
        }
    }
}
