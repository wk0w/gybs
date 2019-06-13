using Microsoft.Extensions.DependencyInjection;
using System;

namespace Gybs
{
    /// <summary>
    /// Service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Gybs services.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="builderAction">Builder action used to configure additional services.</param>
        /// <returns>Services.</returns>
        public static IServiceCollection AddGybs(this IServiceCollection serviceCollection, Action<GybsServicesBuilder> builderAction)
        {
            if (builderAction == null) throw new ArgumentNullException(nameof(builderAction), "Builder action is required.");

            builderAction.Invoke(new GybsServicesBuilder(serviceCollection));
            return serviceCollection;
        }
    }
}
