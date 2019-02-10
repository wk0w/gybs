using System;

namespace Gybs.Logic.Validation.Validator.Internal
{
    internal class ValidationRuleConfiguration
    {
        public Type ValidationRuleType { get; }
        public object Data { get; }
        public int? Group { get; set; }
        public int? Priority { get; set; }
        public bool StopIfFailed { get; set; }

        public ValidationRuleConfiguration(Type validationRuleType, object data)
        {
            ValidationRuleType = validationRuleType;
            Data = data;
        }
    }
}
