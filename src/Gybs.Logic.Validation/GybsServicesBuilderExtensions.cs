using System.Reflection;
using Gybs.DependencyInjection;
using Gybs.Internal;
using Gybs.Logic.Validation.Validator;
using Gybs.Logic.Validation.Validator.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Gybs.Logic.Validation
{
    /// <summary>
    /// <see cref="GybsServicesBuilder"/> extensions.
    /// </summary>
    public static class GybsServicesBuilderExtensions
    {
        /// <summary>
        /// Adds a <see cref="IValidatorFactory"/> which allows to create a <see cref="IValidator"/> instances.
        /// </summary>
        /// <param name="servicesBuilder">The builder.</param>
        /// <returns>The builder.</returns>
        public static GybsServicesBuilder AddValidatorFactory(this GybsServicesBuilder servicesBuilder)
        {
            ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance
                .AddScoped<IValidatorFactory, ValidatorFactory>();

            return servicesBuilder;
        }

        /// <summary>
        /// Adds all implementations of <see cref="IValidationRule{TValidationData}"/> from the assembly.
        /// </summary>
        /// <param name="servicesBuilder">The builder.</param>
        /// <param name="assembly">The assembly. If not provided, <see cref="Assembly.GetCallingAssembly"/> is used.</param>
        /// <returns>The builder.</returns>
        public static GybsServicesBuilder AddValidationRules(this GybsServicesBuilder servicesBuilder, Assembly assembly = null)
        {
            ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance
                .AddTypesImplementingInterfaceFromAssembly(typeof(IValidationRule<>), assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Transient);
            return servicesBuilder;
        }
    }
}
