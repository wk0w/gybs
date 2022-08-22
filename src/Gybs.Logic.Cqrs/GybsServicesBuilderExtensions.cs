using Gybs.DependencyInjection;
using Gybs.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Gybs.Logic.Cqrs;

/// <summary>
/// <see cref="GybsServicesBuilder"/> extensions.
/// </summary>
public static class GybsServicesBuilderExtensions
{
    /// <summary>
    /// Adds all query and command handlers from the assembly.
    /// </summary>
    /// <param name="servicesBuilder">The builder.</param>
    /// <param name="assembly">The assembly. If not provided, <see cref="Assembly.GetCallingAssembly"/> is used.</param>
    /// <returns>The builder.</returns>
    [Obsolete("Given the possibility of multiple ServiceAttribute instances with different groups defined, usage of this method is discouraged.")]
    public static GybsServicesBuilder AddCqrsHandlers(this GybsServicesBuilder servicesBuilder, Assembly? assembly = null)
    {
        var serviceCollection = ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance;
        serviceCollection.AddTypesImplementingInterfaceFromAssembly(typeof(IQueryHandler<,>), assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Transient);
        serviceCollection.AddTypesImplementingInterfaceFromAssembly(typeof(ICommandHandler<>), assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Transient);
        serviceCollection.AddTypesImplementingInterfaceFromAssembly(typeof(ICommandHandler<,>), assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Transient);

        return servicesBuilder;
    }
}
