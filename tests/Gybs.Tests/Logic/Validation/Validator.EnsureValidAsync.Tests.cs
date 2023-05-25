using FluentAssertions;
using Gybs.DependencyInjection.Services;
using Gybs.Extensions;
using Gybs.Logic.Validation;
using Gybs.Logic.Validation.Internal;
using Gybs.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Gybs.Tests.Logic.Validation;

public class ValidatorEnsureValidAsyncTests
{
    private const string ServiceAttributeGroup = nameof(ValidatorEnsureValidAsyncTests);

    [Fact]
    public async Task ForSuccessfulValidationShouldPass()
    {
        var validator = CreateValidator();

        await validator
            .Require<SucceededIfNotNullRule>().WithData(string.Empty)
            .Require<SucceededIfNotNullRule>().WithData(() => string.Empty)
            .Require<SucceededRule>().WithData((string)null!)
            .Require<SucceededRule>().WithData(int.MinValue)
            .EnsureValidAsync();
    }

    [Fact]
    public async Task ForFailedValidationShouldThrowException()
    {
        var validator = CreateValidator();

        Func<Task> action = async () => await validator
            .Require<SucceededRule>().WithData(string.Empty)
            .Require<SucceededRule>().WithData(() => int.MinValue)
            .Require<FailedRule>().WithData(() => string.Empty)
            .Require<FailedRule>().WithData(int.MinValue)
            .EnsureValidAsync();

        await action.Should().ThrowAsync<ValidationFailedException>();
    }

    private IValidator CreateValidator()
    {
        var logger = Substitute.For<ILogger<Validator>>();
        var serviceProvider = new DefaultServiceProviderFactory().CreateServiceProvider(
            new ServiceCollection()
                .AddGybs(builder => builder.AddValidator().AddAttributeServices(group: ServiceAttributeGroup))
        );

        return new Validator(logger, serviceProvider);
    }

    [TransientService(ServiceAttributeGroup)]
    private class SucceededRule : IValidationRule<string>, IValidationRule<int>
    {
        public Task<IResult> ValidateAsync(string data)
        {
            return Result.Success().ToCompletedTask();
        }

        public Task<IResult> ValidateAsync(int data)
        {
            return Result.Success().ToCompletedTask();
        }
    }

    [TransientService(ServiceAttributeGroup)]
    private class SucceededIfNotNullRule : IValidationRule<string?>
    {
        public Task<IResult> ValidateAsync(string? data)
        {
            if (data is null)
            {
                return Result.Failure(new ResultErrorsDictionary().Add("key", "value")).ToCompletedTask();
            }

            return Result.Success().ToCompletedTask();
        }
    }

    [TransientService(ServiceAttributeGroup)]
    private class FailedRule : IValidationRule<string>, IValidationRule<int>
    {
        public Task<IResult> ValidateAsync(string data)
        {
            return Result.Failure(new ResultErrorsDictionary().Add("key", "value")).ToCompletedTask();
        }

        public Task<IResult> ValidateAsync(int data)
        {
            return Result.Failure(new ResultErrorsDictionary().Add("key", "value")).ToCompletedTask();
        }
    }
}
