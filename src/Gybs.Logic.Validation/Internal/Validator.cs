using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gybs.Extensions;
using Gybs.Results;
using Microsoft.Extensions.Logging;

namespace Gybs.Logic.Validation.Internal
{
    internal class Validator : IValidator
    {
        private readonly ILogger<Validator> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly List<ConfiguredValidationRule> _validationRules = new List<ConfiguredValidationRule>();
        
        public Validator(
            ILogger<Validator> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public ConfiguredValidationRule<TValidationRule> Require<TValidationRule>()
            where TValidationRule : IValidationRule
        {
            var rule = new ConfiguredValidationRule<TValidationRule>(this);
            _validationRules.Add(rule);
            return rule;
        }

        public async Task<IResult> ValidateAsync()
        {
            var groups = _validationRules
                .OrderByDescending(c => c.Priority)
                .GroupBy(c => c.Group)
                .OrderBy(g => g.Key ?? long.MaxValue)
                .ToList();

            _validationRules.Clear();

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                var effectiveRulesOrder = groups
                    .Select(g => $"{g.Key?.ToString() ?? "none"}: {string.Join(", ", g.Select(r => $"{r.RuleType.FullName} ({r.Priority}{(r.StopIfFailed ? "!" : string.Empty)})"))}");
                _logger.LogDebug($"Effective rules order:\n      * {string.Join("\n      * ", effectiveRulesOrder)}.");
            }

            var results = new List<IResult>();

            foreach (var group in groups)
            {
                var hasRuleFailed = false;

                foreach (var rule in group)
                {
                    var result = await rule.ValidateAsync(_serviceProvider).ConfigureAwait(false);

                    if (result.HasSucceeded)
                    {
                        continue;
                    }

                    results.Add(result);
                    hasRuleFailed = true;

                    if (rule.StopIfFailed)
                    {
                        break;
                    }
                }

                if (hasRuleFailed)
                {
                    break;
                }
            }

            if (results.Count == 0)
            {
                return Result.Factory.CreateSuccess(default);
            }

            var hasAggregationSucceeded = results.All(r => r.HasSucceeded);
            var aggregatedErrors = results
                .SelectMany(r => r.Errors)
                .GroupBy(r => r.Key)
                .ToDictionary(g => g.Key, g => g.SelectMany(r => r.Value).ToList().CastToReadOnly());
            var aggregatedMetadata = results
                .SelectMany(r => r.Metadata)
                .GroupBy(k => k.Key)
                .ToDictionary(g => g.Key, g => g.First().Value);

            return hasAggregationSucceeded
                ? Result.Factory.CreateSuccess(aggregatedMetadata)
                : Result.Factory.CreateFailure(aggregatedErrors, aggregatedMetadata);
        }

        public async Task EnsureValidAsync()
        {
            var result = await ValidateAsync().ConfigureAwait(false);

            if (!result.HasSucceeded)
            {
                throw new ValidationFailedException(result);
            }
        }
    }
}
