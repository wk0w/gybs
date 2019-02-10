﻿using System.Threading.Tasks;
using Gybs.Logic.Validation.Internal;

namespace Gybs.Logic.Validation.Validator
{
    /// <summary>
    /// Combines and invokes multiple validation rules.
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Adds new validation rule.
        /// </summary>
        /// <typeparam name="TValidationRule">The type of the validation rule.</typeparam>
        /// <returns>The validation rule builder.</returns>
        IValidationRuleBuilder<TValidationRule> Require<TValidationRule>()
            where TValidationRule : IValidationRule;

        /// <summary>
        /// Invokes all rules from the validation builder and aggregates the results.
        /// </summary>
        /// <returns>The result of validation.</returns>
        Task<IResult> ValidateAsync();
    }
}
