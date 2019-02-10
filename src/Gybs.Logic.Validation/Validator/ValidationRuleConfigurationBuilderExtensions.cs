using System.Threading.Tasks;
using Gybs.Logic.Validation.Internal;

namespace Gybs.Logic.Validation.Validator
{
    /// <summary>
    /// <see cref="ValidationRuleConfigurationBuilder"/> extensions.
    /// </summary>
    public static class ValidationRuleConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds new validation rule.
        /// </summary>
        /// <typeparam name="TValidationRule">Validation rule type.</typeparam>
        /// <param name="validationRuleConfigurationBuilder">Configuration builder.</param>
        /// <returns>Validation rule builder.</returns>
        public static IValidationRuleBuilder<TValidationRule> Require<TValidationRule>(this ValidationRuleConfigurationBuilder validationRuleConfigurationBuilder)
            where TValidationRule : IValidationRule
        {
            return validationRuleConfigurationBuilder.Build().Require<TValidationRule>();
        }

        /// <summary>
        /// Invokes all rules from the validation builder and aggregates the results.
        /// </summary>
        /// <param name="validationRuleConfigurationBuilder">The configuration builder.</param>
        /// <returns>The validation result.</returns>
        public static Task<IResult> ValidateAsync(this ValidationRuleConfigurationBuilder validationRuleConfigurationBuilder)
        {
            return validationRuleConfigurationBuilder.Build().ValidateAsync();
        }
    }
}