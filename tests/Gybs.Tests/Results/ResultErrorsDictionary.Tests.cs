using FluentAssertions;
using Gybs;
using Gybs.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Gybs.Tests.Results
{
    public class ResultErrorsDictionaryTests
    {
        [Fact]
        public void ForNewStringKeyShouldAddNewError()
        {
            var errors = new ResultErrorsDictionary();
            errors.Add("key", "value");
            errors.ToDictionary().Should().BeEquivalentTo(new Dictionary<string, string[]> { ["key"] = new[] { "value" } });
        }

        [Fact]
        public void ForAlreadyExistingStringKeyShouldAddNewErrorMessage()
        {
            var errors = new ResultErrorsDictionary();
            errors.Add("key", "value");
            errors.Add("key", "value2");
            errors.ToDictionary().Should().BeEquivalentTo(new Dictionary<string, string[]> { ["key"] = new[] { "value", "value2" } });
        }

        [Fact]
        public void ForKeyFromClassPropertyShouldAddNewErrorWithProperName()
        {
            var errors = new ResultErrorsDictionary();
            errors.Add<Model>(m => m.PropertyA, "value");
            errors.ToDictionary().Should().BeEquivalentTo(new Dictionary<string, string[]> { [$"Model.{nameof(Model.PropertyA)}"] = new[] { "value" } });
        }

        [Fact]
        public void ForKeyFromNestedClassMixedWithStructsPropertyShouldAddNewErrorWithProperName()
        {
            var errors = new ResultErrorsDictionary();
            errors.Add<Model>(m => m.PropertyB.PropertyC.PropertyD, "value");
            errors.ToDictionary().Should().BeEquivalentTo(new Dictionary<string, string[]> { [$"Model.{nameof(Model.PropertyB)}.{nameof(Model.PropertyB.PropertyC)}.{nameof(Model.PropertyB.PropertyC.PropertyD)}"] = new[] { "value" } });
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
