using FluentAssertions;
using Gybs.DependencyInjection.Services;
using Gybs.Extensions;
using Gybs.Logic.Validation;
using Gybs.Logic.Validation.Internal;
using Gybs.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Gybs.Tests.Logic.Validation;

public class ValidatorValidateAsyncTests
{
    private const string ServiceAttributeGroup = nameof(ValidatorValidateAsyncTests);

    [Fact]
    public async Task ForSuccessfulValidationShouldSucceeded()
    {
        var validator = CreateValidator();

        var result = await validator
            .Require<SucceededIfNotNullRule>().WithData(string.Empty)
            .Require<SucceededIfNotNullRule>().WithData(() => string.Empty)
            .Require<SucceededRule>().WithData((string)null!)
            .Require<SucceededRule>().WithData(int.MinValue)
            .ValidateAsync();

        result.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task ForFailedValidationShouldFail()
    {
        var errors = new Dictionary<string, IReadOnlyCollection<string>> { ["key"] = new List<string> { "value_str", "value_int" } };
        var validator = CreateValidator();

        var result = await validator
            .Require<SucceededRule>().WithData(string.Empty)
            .Require<SucceededRule>().WithData(() => int.MinValue)
            .Require<FailedRule>().WithData(() => string.Empty)
            .Require<FailedRule>().WithData(int.MinValue)
            .ValidateAsync();

        result.HasSucceeded.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public async Task ForMixedReturnTypeValidationRuleShouldSucceeded()
    {
        var validator = CreateValidator();

        var result = await validator
            .Require<MixedReturnTypeRule>().WithData(string.Empty)
            .Require<MixedReturnTypeRule>().WithData(() => string.Empty)
            .ValidateAsync();

        result.HasSucceeded.Should().BeTrue();
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

    [TransientService((ServiceAttributeGroup))]
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

    [TransientService((ServiceAttributeGroup))]
    private class FailedRule : IValidationRule<string>, IValidationRule<int>
    {
        public Task<IResult> ValidateAsync(string data)
        {
            return Result.Failure(new ResultErrorsDictionary().Add("key", "value_str")).ToCompletedTask();
        }

        public Task<IResult> ValidateAsync(int data)
        {
            return Result.Failure(new ResultErrorsDictionary().Add("key", "value_int")).ToCompletedTask();
        }
    }

    [TransientService((ServiceAttributeGroup))]
    private class MixedReturnTypeRule : IValidationRule<string>, IValueValidationRule<int>
    {
        public Task<IResult> ValidateAsync(string data)
        {
            return Result.Success().ToCompletedTask();
        }

        public ValueTask<IResult> ValidateAsync(int data)
        {
            return new ValueTask<IResult>(Result.Success());
        }
    }
}
