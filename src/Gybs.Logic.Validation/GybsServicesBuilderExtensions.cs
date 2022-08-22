using Gybs.DependencyInjection;
using Gybs.Internal;
using Gybs.Logic.Validation.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;

namespace Gybs.Logic.Validation;

/// <summary>
/// <see cref="GybsServicesBuilder"/> extensions.
/// </summary>
public static class GybsServicesBuilderExtensions
{
    /// <summary>
    /// Adds a <see cref="IValidator"/> default implementation which allows to aggregate validation rules.
    /// </summary>
    /// <param name="servicesBuilder">The builder.</param>
    /// <returns>The builder.</returns>
    public static GybsServicesBuilder AddValidator(this GybsServicesBuilder servicesBuilder)
    {
        var serviceCollection = ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance;
        serviceCollection.TryAddTransient<IValidator, Validator>();

        return servicesBuilder;
    }
    
    /// <summary>
    /// Adds a <see cref="IValidator"/> default implementation and all implementations of <see cref="IValidationRule{TValidationData}"/> which allows to aggregate validation rules.
    /// </summary>
    /// <param name="servicesBuilder">The builder.</param>
    /// <param name="assembly">The assembly. If not provided, <see cref="Assembly.GetCallingAssembly"/> is used.</param>
    /// <returns>The builder.</returns>
    [Obsolete("Given the possibility of multiple ServiceAttribute instances with different groups defined, usage of this method is discouraged.")]
    public static GybsServicesBuilder AddValidation(this GybsServicesBuilder servicesBuilder, Assembly? assembly = null)
    {
        var serviceCollection = ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance;
        serviceCollection.TryAddTransient<IValidator, Validator>();

        serviceCollection.AddTypesImplementingInterfaceFromAssembly(
            typeof(IValidationRule<>),
            assembly ?? Assembly.GetCallingAssembly(),
            ServiceLifetime.Transient
        );

        return servicesBuilder;
    }
}
