using System.Reflection;
using Gybs.DependencyInjection;
using Gybs.Internal;
using Gybs.Logic.Validation.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gybs.Logic.Validation
{
    /// <summary>
    /// <see cref="GybsServicesBuilder"/> extensions.
    /// </summary>
    public static class GybsServicesBuilderExtensions
    {
        /// <summary>
        /// Adds a <see cref="IValidator"/> and all implementations of <see cref="IValidationRule{TValidationData}"/> which allows to aggregate validation rules.
        /// </summary>
        /// <param name="servicesBuilder">The builder.</param>
        /// <param name="assembly">The assembly. If not provided, <see cref="Assembly.GetCallingAssembly"/> is used.</param>
        /// <returns>The builder.</returns>
        public static GybsServicesBuilder AddValidation(this GybsServicesBuilder servicesBuilder, Assembly assembly = null)
        {
            servicesBuilder.AddDefaultResultFactory();

            var serviceCollection = ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance;
            serviceCollection.TryAddTransient<IValidator, Validator>();

            serviceCollection.AddTypesImplementingInterfaceFromAssembly(
                typeof(IValidationRule<>),
                assembly ?? Assembly.GetCallingAssembly(),
                ServiceLifetime.Transient);

            return servicesBuilder;
        }
    }
}
