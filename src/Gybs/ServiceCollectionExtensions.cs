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
        /// <param name="builder">Builder action used to configure additional services.</param>
        /// <returns>Services.</returns>
        public static IServiceCollection AddGybs(this IServiceCollection serviceCollection, Action<GybsServicesBuilder> builder)
        {
            if (builder is null) throw new ArgumentNullException(nameof(builder), "Builder action is required.");

            builder.Invoke(new GybsServicesBuilder(serviceCollection));
            return serviceCollection;
        }
    }
}
