using Gybs.Logic.Validation.Internal;

namespace Gybs.Logic.Validation.Validator
{
    /// <summary>
    /// Represents a builder for specific validation rule.
    /// </summary>
    /// <typeparam name="TValidationRule">The type of the validation rule.</typeparam>
    public interface IValidationRuleBuilder<out TValidationRule>
        where TValidationRule : IValidationRule
    {        
    }    
}
