using Gybs.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gybs.Logic.Events.InMemory;

/// <summary>
/// <see cref="GybsServicesBuilder"/> extensions.
/// </summary>
public static class GybsServicesBuilderExtensions
{
    /// <summary>
    /// Adds the singleton in-memory implementation of <see cref="IEventBus"/>.
    /// </summary>
    /// <param name="servicesBuilder">The builder.</param>
    /// <returns>The builder.</returns>
    public static GybsServicesBuilder AddInMemoryEventBus(this GybsServicesBuilder servicesBuilder)
    {
        ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance
            .TryAddSingleton<IEventBus, InMemoryEventBus>();
        return servicesBuilder;
    }
}
