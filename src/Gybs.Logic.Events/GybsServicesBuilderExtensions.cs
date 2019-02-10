using Gybs.Internal;
using Gybs.Logic.Events.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Gybs.Logic.Events
{
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
                .AddSingleton<IEventBus, InMemoryEventBus>();
            return servicesBuilder;
        }
    }
}
