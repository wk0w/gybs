using Gybs.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gybs.DependencyInjection;

/// <summary>
/// <see cref="IServiceCollection"/> extensions for working with assemblies.
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Adds all implementations of the provided interface from the assembly.
    /// </summary>
    /// <remarks>
    /// Registration is done for the type itself and each interface implemented by it, not only the requested one.
    /// </remarks>
    /// <param name="serviceCollection">Service collection.</param>
    /// <param name="interfaceType">The interface type to add.</param>
    /// <param name="assembly">The assembly with implementations.</param>
    /// <param name="serviceLifetime">The service lifetime.</param>
    /// <returns>Service collection.</returns>
    public static IServiceCollection AddTypesImplementingInterfaceFromAssembly(
        this IServiceCollection serviceCollection,
        Type interfaceType,
        Assembly assembly,
        ServiceLifetime serviceLifetime)
    {
        if (!interfaceType.IsInterface) throw new ArgumentException("Provided type needs to be an interface.", nameof(interfaceType));
        if (assembly is null) throw new ArgumentNullException(nameof(assembly));

        var assemblyTypes = assembly.GetTypes().Where(t => !t.IsAbstract);

        foreach (var assemblyType in assemblyTypes)
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

            if (implementedInterfaceTypes.Length is 0)
            {
                continue;
            }

            AddType(serviceCollection, implementedInterfaces, assemblyType, serviceLifetime);
        }

        return serviceCollection;
    }

    /// <summary>
    /// Adds all types marked with provided attribute from the assembly.
    /// </summary>
    /// <remarks>
    /// Registration is done for the type itself and each interface implemented by it, not only the requested one.
    /// </remarks>
    /// <param name="serviceCollection">Service collection.</param>
    /// <param name="attributeType">The attribute type.</param>
    /// <param name="assembly">The assembly with types to add.</param>
    /// <param name="serviceLifetime">The service lifetime.</param>
    /// <returns>Service collection.</returns>
    public static IServiceCollection AddTypesWithAttributeFromAssembly(
        this IServiceCollection serviceCollection,
        Type attributeType,
        Assembly assembly,
        ServiceLifetime serviceLifetime)
    {
        if (attributeType.IsAssignableFrom(typeof(Attribute))) throw new ArgumentException("Provided type needs to be an attribute.", nameof(attributeType));
        if (assembly is null) throw new ArgumentNullException(nameof(assembly));

        var assemblyTypes = assembly.GetTypes().Where(t => !t.IsAbstract);

        foreach (var assemblyType in assemblyTypes)
        {
            var attributes = assemblyType.GetCustomAttributes();

            if (!attributes.Any(a => a.GetType().IsAssignableFrom(attributeType)))
            {
                continue;
            }

            AddType(serviceCollection, assemblyType.GetInterfaces(), assemblyType, serviceLifetime);
        }

        return serviceCollection;
    }

    /// <summary>
    /// Adds all types matched by predicate from the assembly.
    /// </summary>
    /// <remarks>
    /// Registration is done for the type itself and each interface implemented by it, not only the requested one.
    /// </remarks>
    /// <param name="serviceCollection">Service collection.</param>
    /// <param name="predicate">The predicate which returns service lifetime for types to be added or default value otherwise.</param>
    /// <param name="assembly">The assembly with types to add.</param>
    /// <returns>Service collection.</returns>
    public static IServiceCollection AddMatchingTypesFromAssembly(
        this IServiceCollection serviceCollection,
        Func<Type, ServiceLifetime?> predicate,
        Assembly assembly)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        if (assembly is null) throw new ArgumentNullException(nameof(assembly));

        var assemblyTypes = assembly.GetTypes().Where(t => !t.IsAbstract);

        foreach (var assemblyType in assemblyTypes)
        {
            var serviceLifetime = predicate(assemblyType);

            if (serviceLifetime is null)
            {
                continue;
            }

            AddType(serviceCollection, assemblyType.GetInterfaces(), assemblyType, serviceLifetime.Value);
        }

        return serviceCollection;
    }

    private static void AddType(IServiceCollection serviceCollection, IEnumerable<Type> interfaceTypes, Type assemblyType, ServiceLifetime serviceLifetime)
    {
        serviceCollection.Add(new ServiceDescriptor(assemblyType, assemblyType, serviceLifetime));
        interfaceTypes.ForEach(i => serviceCollection.Add(new ServiceDescriptor(i, p => p.GetService(assemblyType)!, serviceLifetime)));
    }
}
