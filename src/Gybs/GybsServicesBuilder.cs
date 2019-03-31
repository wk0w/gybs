using Gybs.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Gybs
{    
    /// <summary>
    /// Represents a builder for Gybs services.
    /// </summary>
    public sealed class GybsServicesBuilder : IInfrastructure<IServiceCollection>
    {
        private readonly IServiceCollection _services;
        IServiceCollection IInfrastructure<IServiceCollection>.Instance => _services;

        internal GybsServicesBuilder(IServiceCollection services)
        {
            _services = services;
        }
    }

    /// <summary>
    /// Gybs services builder extensions.
    /// </summary>
    public static class GybsServicesBuilderExtensions
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
