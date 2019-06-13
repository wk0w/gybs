﻿using System;
using Gybs.Logic.Validation.Internal;

namespace Gybs.Logic.Validation
{
    /// <summary>
    /// Represents a validation rule with execution configuration.
    /// </summary>
    /// <typeparam name="TValidationRule">Type of the validation rule.</typeparam>
    public sealed class ConfiguredValidationRule<TValidationRule> : ConfiguredValidationRule, IConfiguredValidationRule<TValidationRule>
        where TValidationRule : IValidationRule
    {
        public ConfiguredValidationRule(IValidator validator)
            : base(validator, typeof(TValidationRule))
        {
        }
    }

    /// <summary>
    /// Configured validation rule extensions.
    /// </summary>
    public static class ConfiguredValidationRuleExtensions
    {
        /// <summary>
        /// Sets the data passed to the validation rule.
        /// </summary>
        /// <param name="rule">Validation rule.</param>
        /// <param name="data">Data to validate.</param>
        /// <typeparam name="TData">Type of the data to validate.</typeparam>
        /// <returns>The validator.</returns>
        public static IValidator WithData<TData>(this IConfiguredValidationRule<IValidationRule<TData>> rule, TData data)
        {
            if (!(rule is ConfiguredValidationRule castedRule)) throw new ArgumentException("Rule is not of ConfiguredValidationRule type.", nameof(rule));

            castedRule.Data = data;
            return castedRule.Validator;
        }

        /// <summary>
        /// Configures additional options for the validation rule.
        /// </summary>
        /// <param name="rule">Validation rule.</param>
        /// <param name="options">Function used to configure the options.</param>
        /// <typeparam name="TValidationRule">Type of the validation rule.</typeparam>
        /// <returns>Configured validation rule.</returns>
        public static ConfiguredValidationRule<TValidationRule> WithOptions<TValidationRule>(this ConfiguredValidationRule<TValidationRule> rule, Action<ConfiguredValidationRuleOptionsBuilder> options)
            where TValidationRule : IValidationRule
        {
            var builder = new ConfiguredValidationRuleOptionsBuilder(rule);
            options?.Invoke(builder);
            return rule;
        }
    }
}