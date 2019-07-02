namespace Gybs.DependencyInjection.Services
{
    /// <summary>
    /// Represents a service which is registered within dependency injection container as singleton.
    /// </summary>
    public interface ISingletonService
    {
    }

    /// <summary>
    /// Represents a service which is registered within dependency injection container with scoped lifetime.
    /// </summary>
    public interface IScopedService
    {
    }

    /// <summary>
    /// Represents a service which is registered within dependency injection container with transient lifetime.
    /// </summary>
    public interface ITransientService
    {
    }
}
