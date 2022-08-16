using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;

namespace Gybs.Logic.Validation.Internal;

/// <summary>
/// Represents a validation rule with execution configuration.
/// </summary>
/// <typeparam name="TValidationRule">Type of the validation rule.</typeparam>
public interface IConfiguredValidationRule<out TValidationRule>
    where TValidationRule : IValidationRule
{
}

/// <summary>
/// Represents a validation rule with execution configuration.
/// </summary>
public abstract class ConfiguredValidationRule
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> ValidateAsyncMethodInfos = new();

    internal IValidator Validator { get; }
    internal Type RuleType { get; }
    internal int? Group { get; set; }
    internal int? Priority { get; set; }
    internal bool StopIfFailed { get; set; }
    internal object? Data { get; set; }

    private protected ConfiguredValidationRule(IValidator validator, Type ruleType)
    {
        Validator = validator;
        RuleType = ruleType;
    }

    internal Task<IResult> ValidateAsync(IServiceProvider serviceProvider)
    {
        var rule = serviceProvider.GetService(RuleType);

        if (rule is null) throw new InvalidOperationException($"Validation rule of {RuleType.FullName} type cannot be resolved.");

        var validateAsyncMethodInfo = ValidateAsyncMethodInfos.GetOrAdd(
            RuleType,
            t => t.GetMethod(nameof(IValidationRule<object>.ValidateAsync))!
        );

        return (Task<IResult>)validateAsyncMethodInfo.Invoke(rule, new[] { Data })!;
    }
}
