using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Gybs
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Gybs services.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <returns>Gybs services builder.</returns>
        public static GybsServicesBuilder AddGybs(this IServiceCollection services)
        {
            return new GybsServicesBuilder(services);
        }

        /// <summary>
        /// Adds all implementations of the provided interfaces from the assembly.
        /// </summary>
        /// <remarks>
        /// Registration is done for each interface of class, not only the requested one.
        /// </remarks>
        /// <param name="serviceCollection">Service collection.</param>
        /// <param name="interfaceTypes">The interface types to add.</param>
        /// <param name="assembly">The assembly with implementations.</param>
        /// <param name="serviceLifetime">The service lifetime.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddImplementationsFromAssembly(
            this IServiceCollection serviceCollection,
            Type[] interfaceTypes, 
            Assembly assembly, 
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            if (interfaceTypes.Any(i => !i.IsInterface))
            {
                throw new ArgumentException("All provided types need to be and interface.", nameof(interfaceTypes));
            }

            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var assemblyTypes = assembly.GetTypes().Where(t => !t.IsAbstract);

            foreach (var assemblyType in assemblyTypes)
            {
                foreach (var interfaceType in interfaceTypes)
                {
                    var implementedInterfaces = assemblyType.GetInterfaces();
                    Type[] implementedInterfaceTypes;


                    if (interfaceType.IsGenericType)
                    {
                        implementedInterfaceTypes = implementedInterfaces
                            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType)
                            .ToArray();
                    }
                    else
                    {
                        implementedInterfaceTypes = implementedInterfaces
                            .Where(i => interfaceType.IsAssignableFrom(i))
                            .ToArray();
                    }

                    if (!implementedInterfaceTypes.Any())
                    {
                        continue;
                    }

                    foreach (var implementedInterface in implementedInterfaces)
                    {
                        serviceCollection.Add(new ServiceDescriptor(implementedInterface, assemblyType, serviceLifetime));
                    }

                    serviceCollection.Add(new ServiceDescriptor(assemblyType, assemblyType, serviceLifetime));

                }                
            }

            return serviceCollection;
        }
    }
}