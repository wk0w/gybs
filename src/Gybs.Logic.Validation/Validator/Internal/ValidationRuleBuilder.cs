using Gybs.Internal;
using Gybs.Logic.Validation.Internal;

namespace Gybs.Logic.Validation.Validator.Internal
{
    internal class ValidationRuleBuilder<TValidationRule> : IValidationRuleBuilder<TValidationRule>, IInfrastructure<Validator>
        where TValidationRule : IValidationRule
    {
        private readonly Validator _validator;
        Validator IInfrastructure<Validator>.Instance => _validator;

        public ValidationRuleBuilder(Validator validator)
        {
            _validator = validator;
        }
    }
}
