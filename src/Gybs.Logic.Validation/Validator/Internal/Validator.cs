using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Gybs.Extensions;
using Gybs.Logic.Validation.Internal;
using Gybs.Results;
using Microsoft.Extensions.Logging;

namespace Gybs.Logic.Validation.Validator.Internal
{
    internal class Validator : IValidator
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> ValidateAsyncMethodInfos = new ConcurrentDictionary<Type, MethodInfo>();
        private readonly ILogger<Validator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly List<ValidationRuleConfiguration> _validationRuleConfigurations = new List<ValidationRuleConfiguration>();

        public Validator(
            ILogger<Validator> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public IValidationRuleBuilder<TValidationRule> Require<TValidationRule>()
            where TValidationRule : IValidationRule
        {
            return new ValidationRuleBuilder<TValidationRule>(this);
        }

        public async Task<IResult> ValidateAsync()
        {
            var groups = _validationRuleConfigurations
                .OrderByDescending(c => c.Priority)
                .GroupBy(c => c.Group)
                .OrderBy(g => g.Key ?? long.MaxValue)
                .ToList();

            var effectiveRulesOrder = groups
                .Select(g => $"{g.Key?.ToString() ?? "none"}: {string.Join(", ", g.Select(r => $"{r.ValidationRuleType.FullName} ({r.Priority}{(r.StopIfFailed ? "!" : string.Empty)})"))}");
            _logger.LogDebug($"Effective rules order:\n      * {string.Join("\n      * ", effectiveRulesOrder)}.");

            var results = new List<IResult>();

            foreach (var group in groups)
            {
                var hasRuleFailed = false;

                foreach (var ruleConfiguration in group)
                {
                    var rule = _serviceProvider.GetService(ruleConfiguration.ValidationRuleType);

                    if (rule == null)
                    {
                        throw new InvalidOperationException($"Validation rule of {ruleConfiguration.ValidationRuleType.FullName} type cannot be resolved.");
                    }

                    var validateAsyncMethodInfo = ValidateAsyncMethodInfos.GetOrAdd(
                        ruleConfiguration.ValidationRuleType,
                        t => t.GetMethod(nameof(IValidationRule<object>.ValidateAsync)));
                    var result = await (Task<IResult>)validateAsyncMethodInfo.Invoke(rule, new[] { ruleConfiguration.Data });

                    if (result.HasSucceeded)
                    {
                        continue;
                    }

                    results.Add(result);
                    hasRuleFailed = true;

                    if (ruleConfiguration.StopIfFailed)
                    {
                        break;
                    }
                }

                if (hasRuleFailed)
                {
                    break;
                }
            }

            if (!results.Any())
            {
                return Result.Success();
            }

            IResult CreateResult(bool hasSucceeded, IReadOnlyDictionary<string, IReadOnlyCollection<string>> errors, IReadOnlyDictionary<string, object> metadata)
                => (hasSucceeded ? Result.Success() : Result.Failure(errors)).AddMetadata(metadata);

            var hasAggregationSucceeded = results.All(r => r.HasSucceeded);
            var aggregatedErrors = results
                .SelectMany(r => r.Errors)
                .GroupBy(r => r.Key)
                .ToDictionary(g => g.Key, g => g.SelectMany(r => r.Value).ToList().CastToReadOnly());
            var aggregatedMetadata = results
                .SelectMany(r => r.Metadata)
                .GroupBy(k => k.Key)
                .ToDictionary(g => g.Key, g => g.First().Value);

            return CreateResult(hasAggregationSucceeded, aggregatedErrors, aggregatedMetadata);
        }

        public void AddRule(ValidationRuleConfiguration validationRuleConfiguration)
        {
            _validationRuleConfigurations.Add(validationRuleConfiguration);
        }
    }
}
