using Microsoft.Extensions.DependencyInjection;
using System;

namespace Gybs.DependencyInjection.Services;

/// <summary>
/// Represents a service which is registered within dependency injection container.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public abstract class ServiceAttribute : Attribute
{
    /// <summary>
    /// Gets the group which can be used to register selected types only.
    /// </summary>
    public string? Group { get; }

    /// <summary>
    /// Gets the lifetime of the registered service.
    /// </summary>
    internal ServiceLifetime ServiceLifetime { get; }

    private protected ServiceAttribute(string? group, ServiceLifetime serviceLifetime)
    {
        Group = group;
        ServiceLifetime = serviceLifetime;
    }
}

/// <summary>
/// Represents a service which is registered within dependency injection container as singleton.
/// </summary>
public sealed class SingletonServiceAttribute : ServiceAttribute
{
    /// <summary>
    /// Creates an instance of the attribute.
    /// </summary>
    /// <param name="group">Container registration group.</param>
    public SingletonServiceAttribute(string? group = null)
        : base(group, ServiceLifetime.Singleton)
    {
    }
}

/// <summary>
/// Represents a service which is registered within dependency injection container with scoped lifetime.
/// </summary>
public sealed class ScopedServiceAttribute : ServiceAttribute
{
    /// <summary>
    /// Creates an instance of the attribute.
    /// </summary>
    /// <param name="group">Container registration group.</param>
    public ScopedServiceAttribute(string? group = null)
        : base(group, ServiceLifetime.Scoped)
    {
    }
}

/// <summary>
/// Represents a service which is registered within dependency injection container with transient lifetime.
/// </summary>
public sealed class TransientServiceAttribute : ServiceAttribute
{
    /// <summary>
    /// Creates an instance of the attribute.
    /// </summary>
    /// <param name="group">Container registration group.</param>
    public TransientServiceAttribute(string? group = null)
        : base(group, ServiceLifetime.Transient)
    {
    }
}
