using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Gybs.Results;
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
        public void ForKeyFromNestedCollectionsWithMethodsShouldAddNewErrorWithProperName()
        {
            var errors = new ResultErrorsDictionary();
            errors.Add<Model>(m => m.CollectionA.First().CollectionB.First().PropertyD, "value");
            errors.Should().BeEquivalentTo(new Dictionary<string, string[]> { [$"Model.{nameof(Model.CollectionA)}.{nameof(Model.PropertyB.CollectionB)}.{nameof(Model.PropertyB.PropertyC.PropertyD)}"] = new[] { "value" } });
        }

        [Fact]
        public void ForKeyFromNestedCollectionsWithIndexersShouldAddNewErrorWithProperName()
        {
            var errors = new ResultErrorsDictionary();
            errors.Add<Model>(m => m.CollectionA[0].CollectionB[0].PropertyD, "value");
            errors.Should().BeEquivalentTo(new Dictionary<string, string[]> { [$"Model.{nameof(Model.CollectionA)}.{nameof(Model.PropertyB.CollectionB)}.{nameof(Model.PropertyB.PropertyC.PropertyD)}"] = new[] { "value" } });
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
            new { result, value }.Should().BeEquivalentTo(new { result = false, value = new string[0] });
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

            public InnerModel PropertyB { get; set; } = new InnerModel();

            public IList<InnerModel> CollectionA { get; set; } = new List<InnerModel>();

            public class InnerModel
            {
                public IList<InnerModelInnerStruct> CollectionB { get; set; } = new List<InnerModelInnerStruct>();
                public InnerModelInnerStruct PropertyC { get; set; }

                public struct InnerModelInnerStruct
                {
                    public string PropertyD { get; set; }
                }
            }
        }
    }
}
