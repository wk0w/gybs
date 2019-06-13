using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Gybs.Extensions;
using Gybs.Logic.Validation;
using Gybs.Logic.Validation.Internal;
using Gybs.Results;
using Gybs.Results.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Gybs.Tests.Logic.Validation
{
    public class ValidatorTests
    {
        [Fact]
        public async Task ForSuccessfulValidationShouldSucceeded()
        {
            var validator = CreateValidator();

            var result = await validator
                .Require<SucceededRule>().WithData(string.Empty)
                .Require<SucceededRule>().WithData(string.Empty)
                .ValidateAsync();

            result.HasSucceeded.Should().BeTrue();
        }

        [Fact]
        public async Task ForFailedValidationShouldFail()
        {
            var errors = new Dictionary<string, IReadOnlyCollection<string>> { ["key"] = new List<string> { "value" } };
            var validator = CreateValidator();

            var result = await validator
                .Require<SucceededRule>().WithData(string.Empty)
                .Require<FailedRule>().WithData(string.Empty)
                .ValidateAsync();

            result.HasSucceeded.Should().BeFalse();
            result.Errors.Should().BeEquivalentTo(errors);
        }

        private IValidator CreateValidator()
        {
            var logger = Substitute.For<ILogger<Validator>>();
            var serviceProvider = new DefaultServiceProviderFactory().CreateServiceProvider(
                new ServiceCollection()
                    .AddGybs(builder => builder.AddValidation())
            );

            return new Validator(logger, serviceProvider, new ResultFactory());
        }

        private class SucceededRule : IValidationRule<string>
        {
            public Task<IResult> ValidateAsync(string data)
            {
                return Result.Success().ToCompletedTask();
            }
        }

        private class FailedRule : IValidationRule<string>
        {
            public Task<IResult> ValidateAsync(string data)
            {
                return Result.Failure(new ResultErrorsDictionary().Add("key", "value")).ToCompletedTask();
            }
        }
    }
}