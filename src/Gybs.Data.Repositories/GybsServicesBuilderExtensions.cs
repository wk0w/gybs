using Gybs.DependencyInjection;
using Gybs.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Gybs.Data.Repositories;

/// <summary>
/// <see cref="GybsServicesBuilder"/> extensions.
/// </summary>
public static class GybsServicesBuilderExtensions
{
    /// <summary>
    /// Adds <see cref="IUnitOfWork"/> implementations from the assembly.
    /// </summary>
    /// <param name="servicesBuilder">The builder.</param>
    /// <param name="serviceLifetime">The lifetime of registered service.</param>
    /// <param name="assembly">The assembly. If not provided, <see cref="Assembly.GetCallingAssembly"/> is used.</param>
    /// <returns>The builder.</returns>
    [Obsolete("Given the possibility of multiple ServiceAttribute instances with different groups defined, usage of this method is discouraged.")]
    public static GybsServicesBuilder AddUnitOfWork(this GybsServicesBuilder servicesBuilder, ServiceLifetime serviceLifetime, Assembly? assembly = null)
    {
        ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance
            .AddTypesImplementingInterfaceFromAssembly(typeof(IUnitOfWork), assembly ?? Assembly.GetCallingAssembly(), serviceLifetime);
        return servicesBuilder;
    }

    /// <summary>
    /// Adds <see cref="IRepository{TEntity}"/> implementations from the assembly.
    /// </summary>
    /// <param name="servicesBuilder">The builder.</param>
    /// <param name="serviceLifetime">The lifetime of registered service.</param>
    /// <param name="assembly">The assembly. If not provided, <see cref="Assembly.GetCallingAssembly"/> is used.</param>
    /// <returns>The builder.</returns>
    [Obsolete("Given the possibility of multiple ServiceAttribute instances with different groups defined, usage of this method is discouraged.")]
    public static GybsServicesBuilder AddRepositories(this GybsServicesBuilder servicesBuilder, ServiceLifetime serviceLifetime, Assembly? assembly = null)
    {
        ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance
            .AddTypesImplementingInterfaceFromAssembly(typeof(IRepository<>), assembly ?? Assembly.GetCallingAssembly(), serviceLifetime);
        return servicesBuilder;
    }
}
