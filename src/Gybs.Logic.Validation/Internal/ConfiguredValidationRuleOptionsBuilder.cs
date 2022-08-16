using System;

namespace Gybs.Logic.Validation.Internal;

/// <summary>
/// Represents a builder for the options of the configured validation rule.
/// </summary>
public sealed class ConfiguredValidationRuleOptionsBuilder
{
    private readonly ConfiguredValidationRule _rule;

    internal ConfiguredValidationRuleOptionsBuilder(ConfiguredValidationRule rule)
    {
        _rule = rule;
    }

    /// <summary>
    /// Assigns the group to the rule.
    /// </summary>
    /// <remarks>
    /// All rules without groups are executed within a single default group.
    /// Grouped rules are invoked together and any failure will stop an invocation of the next group.
    /// </remarks>
    /// <param name="group">The enum with <see cref="int"/> as an underlying type.</param>
    /// <returns>The options builder.</returns>
    public ConfiguredValidationRuleOptionsBuilder WithinGroup(Enum group)
    {
        var convertedGroup = Convert.ChangeType(group, group.GetTypeCode()) as int?;

        if (convertedGroup is null) throw new ArgumentException("Underlying type of enum is not int.", nameof(group));

        _rule.Group = convertedGroup.Value;
        return this;
    }

    /// <summary>
    /// Sets the priority for invocation within the group.
    /// </summary>
    /// <remarks>
    /// All rules without groups are executed within a single default group.
    /// Higher value means earlier invocation.
    /// </remarks>
    /// <param name="priority">The enum with <see cref="int"/> as an underlying type.</param>
    /// <returns>The options builder.</returns>
    public ConfiguredValidationRuleOptionsBuilder WithPriority(Enum priority)
    {
        var convertedPriority = Convert.ChangeType(priority, priority.GetTypeCode()) as int?;

        if (convertedPriority is null) throw new ArgumentException("Underlying type of enum is not int.", nameof(priority));

        _rule.Priority = convertedPriority.Value;
        return this;
    }

    /// <summary>
    /// Marks the rule as crucial for the validation.
    /// The process of validation will be stopped if this rule will fail.
    /// </summary>
    /// <returns>The options builder.</returns>
    public ConfiguredValidationRuleOptionsBuilder StopIfFailed()
    {
        _rule.StopIfFailed = true;
        return this;
    }
}
