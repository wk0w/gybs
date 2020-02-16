using System;
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
    public class ValidatorEnsureValidAsyncTests
    {
        [Fact]
        public async Task ForSuccessfulValidationShouldPass()
        {
            var validator = CreateValidator();

            await validator
                .Require<SucceededRule>().WithData(string.Empty)
                .Require<SucceededRule>().WithData(string.Empty)
                .EnsureValidAsync();
        }

        [Fact]
        public async Task ForFailedValidationShouldThrowException()
        {
            var errors = new Dictionary<string, IReadOnlyCollection<string>> { ["key"] = new List<string> { "value" } };
            var validator = CreateValidator();

            Func<Task> action = async () => await validator
                .Require<SucceededRule>().WithData(string.Empty)
                .Require<FailedRule>().WithData(string.Empty)
                .EnsureValidAsync();

            await action.Should().ThrowAsync<ValidationFailedException>();
        }

        private IValidator CreateValidator()
        {
            var logger = Substitute.For<ILogger<Validator>>();
            var serviceProvider = new DefaultServiceProviderFactory().CreateServiceProvider(
                new ServiceCollection()
                    .AddGybs(builder => builder.AddValidation())
            );

            return new Validator(logger, serviceProvider);
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