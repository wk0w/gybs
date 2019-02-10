using System;
using Gybs.Logic.Validation.Validator.Internal;

namespace Gybs.Logic.Validation.Validator
{
    /// <summary>
    /// Represents a builder for the validation rule configuration.
    /// </summary>
    public sealed class ValidationRuleConfigurationBuilder
    {
        private readonly Internal.Validator _validator;
        private readonly ValidationRuleConfiguration _validationRuleConfiguration;

        internal ValidationRuleConfigurationBuilder(Internal.Validator validator, Type validationRuleType, object data)
        {
            _validator = validator;
            _validationRuleConfiguration = new ValidationRuleConfiguration(validationRuleType, data);
        }

        /// <summary>
        /// Assigns the group to the rule. 
        /// </summary>
        /// <remarks>
        /// All rules without groups are executed within a single default group.
        /// Grouped rules are invoked together and any failure will stop an invocation of the next group.
        /// </remarks>
        /// <param name="group">The enum with <see cref="int"/> as an underlying type.</param>
        /// <returns>The configuration builder.</returns>
        public ValidationRuleConfigurationBuilder WithinGroup(Enum group)
        {
            var convertedGroup = Convert.ChangeType(group, group.GetTypeCode()) as int?;

            if (convertedGroup == null)
            {
                throw new ArgumentException("Underlying type of enum is not int.", nameof(group));
            }

            _validationRuleConfiguration.Group = convertedGroup.Value;
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
        /// <returns>The configration builder.</returns>
        public ValidationRuleConfigurationBuilder WithPriority(Enum priority)
        {
            var convertedPriority = Convert.ChangeType(priority, priority.GetTypeCode()) as int?;

            if (convertedPriority == null)
            {
                throw new ArgumentException("Underlying type of enum is not int.", nameof(priority));
            }

            _validationRuleConfiguration.Priority = convertedPriority.Value;
            return this;
        }

        /// <summary>
        /// Marks the rule as crucial for the validation.
        /// The process of validation will be stopped if this rule will fail.
        /// </summary>
        /// <returns>The configuration builder.</returns>
        public ValidationRuleConfigurationBuilder StopIfFailed()
        {
            _validationRuleConfiguration.StopIfFailed = true;
            return this;
        }

        /// <summary>
        /// Completes the build process for the rule.
        /// </summary>
        /// <returns>The validation builder.</returns>
        public IValidator Build()
        {
            _validator.AddRule(_validationRuleConfiguration);
            return _validator;
        }
    }
}
