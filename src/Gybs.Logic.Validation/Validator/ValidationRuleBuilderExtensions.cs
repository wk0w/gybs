using System.Linq;
using Gybs.Internal;

namespace Gybs.Logic.Validation.Validator
{
    /// <summary>
    /// <see cref="IValidationRuleBuilder{TValidationRule}"/> extensions.
    /// </summary>
    public static class ValidationRuleBuilderExtensions
    {
        /// <summary>
        /// Sets the data to validate by the rule.
        /// </summary>
        /// <typeparam name="TData">The type of data to validate.</typeparam>
        /// <param name="validationRuleBuilder">The builder.</param>
        /// <param name="data">The data to validate.</param>
        /// <returns>The configuration builder for the rule.</returns>
        public static ValidationRuleConfigurationBuilder WithData<TData>(this IValidationRuleBuilder<IValidationRule<TData>> validationRuleBuilder, TData data)
        {            
            var validator = ((IInfrastructure<Internal.Validator>)validationRuleBuilder).Instance;
            var ruleType = validationRuleBuilder.GetType().GetGenericArguments().First();
            return new ValidationRuleConfigurationBuilder(validator, ruleType, data);
        }
    }
}
