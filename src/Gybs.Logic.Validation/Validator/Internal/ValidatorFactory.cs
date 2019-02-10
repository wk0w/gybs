using System;
using Microsoft.Extensions.Logging;

namespace Gybs.Logic.Validation.Validator.Internal
{
    internal class ValidatorFactory : IValidatorFactory
    {
        private readonly ILogger<Validator> _validatorLogger;
        private readonly IServiceProvider _serviceProvider;

        public ValidatorFactory(
            ILogger<Validator> validatorLogger,
            IServiceProvider serviceProvider)
        {
            _validatorLogger = validatorLogger;
            _serviceProvider = serviceProvider;
        }

        public IValidator Create()
        {
            return new Validator(_validatorLogger, _serviceProvider);
        }
    }
}