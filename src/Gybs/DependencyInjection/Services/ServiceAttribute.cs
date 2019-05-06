using System;

namespace Gybs.DependencyInjection.Services
{
    /// <summary>
    /// Represents a service which is registered within dependency injection container as singleton.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class SingletonServiceAttribute : Attribute
    {
    }

    /// <summary>
    /// Represents a service which is registered within dependency injection container with scoped lifetime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class ScopedServiceAttribute : Attribute
    {
    }

    /// <summary>
    /// Represents a service which is registered within dependency injection container with transient lifetime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class TransientServiceAttribute : Attribute
    {
    }
}
