using Gybs.Internal;
using Microsoft.Extensions.DependencyInjection;
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
        public static GybsServicesBuilder AddAttributeServices(this GybsServicesBuilder servicesBuilder, Assembly? assembly = default)
        {
            var serviceCollection = ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance;

            serviceCollection.AddTypesWithAttributeFromAssembly(typeof(SingletonServiceAttribute), assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Singleton);
            serviceCollection.AddTypesWithAttributeFromAssembly(typeof(ScopedServiceAttribute), assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Scoped);
            serviceCollection.AddTypesWithAttributeFromAssembly(typeof(TransientServiceAttribute), assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Transient);

            return servicesBuilder;
        }
    }
}
