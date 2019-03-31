using System.Reflection;
using Gybs.DependencyInjection;
using Gybs.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Gybs.Logic.Cqrs
{
    /// <summary>
    /// <see cref="GybsServicesBuilder"/> extensions.
    /// </summary>
    public static class GybsServicesBuilderExtensions
    {
        /// <summary>
        /// Adds all query handlers from the assembly.
        /// </summary>
        /// <param name="servicesBuilder">The builder.</param>        
        /// <param name="assembly">The assembly. If not provided, <see cref="Assembly.GetCallingAssembly"/> is used.</param>
        /// <returns>The builder.</returns>
        public static GybsServicesBuilder AddQueryHandlers(this GybsServicesBuilder servicesBuilder, Assembly assembly = null)
        {
            ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance
                .AddTypesImplementingInterfaceFromAssembly(typeof(IQueryHandler<,>), assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Transient);
            return servicesBuilder;
        }

        /// <summary>
        /// Adds all command handlers from the assembly.
        /// </summary>
        /// <param name="servicesBuilder">The builder.</param>        
        /// <param name="assembly">The assembly. If not provided, <see cref="Assembly.GetCallingAssembly"/> is used.</param>
        /// <returns>The builder.</returns>
        public static GybsServicesBuilder AddCommandHandlers(this GybsServicesBuilder servicesBuilder, Assembly assembly = null)
        {
            var serviceCollection = ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance;
            serviceCollection.AddTypesImplementingInterfaceFromAssembly(typeof(ICommandHandler<>), assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Transient);
            serviceCollection.AddTypesImplementingInterfaceFromAssembly(typeof(ICommandHandler<,>), assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Transient);
                
            return servicesBuilder;
        }
    }
}
