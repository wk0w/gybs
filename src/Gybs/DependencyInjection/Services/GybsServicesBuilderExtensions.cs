using Gybs.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Gybs.DependencyInjection.Services
{
    /// <summary>
    /// Dependency injection extensions for service registration.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Adds all types implementing one of the dependency injection services to the service collection.
        /// </summary>
        /// <param name="servicesBuilder">The builder.</param>
        /// <param name="assembly">The assembly. If not provided, <see cref="Assembly.GetCallingAssembly"/> is used.</param>
        public static GybsServicesBuilder AddInterfaceServices(this GybsServicesBuilder servicesBuilder, Assembly? assembly = default)
        {
            var serviceCollection = ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance;

            serviceCollection.AddTypesImplementingInterfaceFromAssembly(typeof(ISingletonService), assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Singleton);
            serviceCollection.AddTypesImplementingInterfaceFromAssembly(typeof(IScopedService), assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Scoped);
            serviceCollection.AddTypesImplementingInterfaceFromAssembly(typeof(ITransientService), assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Transient);

            return servicesBuilder;
        }

        /// <summary>
        /// Adds all types marked with dependency attributes to the service collection.
        /// </summary>
        /// <param name="servicesBuilder">The builder.</param>
        /// <param name="assembly">The assembly. If not provided, <see cref="Assembly.GetCallingAssembly"/> is used.</param>
        public static GybsServicesBuilder AddAttributeServices(
            this GybsServicesBuilder servicesBuilder,
            Assembly? assembly = default,
            string? group = default)
        {
            var serviceCollection = ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance;
            
            serviceCollection.AddMatchingTypesFromAssembly(
                type =>
                {
                    var attributes = type.GetCustomAttributes()
                        .Where(a => typeof(ServiceAttribute).IsAssignableFrom(a.GetType()))
                        .Cast<ServiceAttribute>()
                        .Where(a => a.Group == group)
                        .ToList();

                    if (!attributes.Any())
                    {
                        return default;
                    }

                    return attributes.Last().ServiceLifetime;
                },
                assembly ?? Assembly.GetCallingAssembly()
            );
            
            return servicesBuilder;
        }
    }
}
