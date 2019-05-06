using System.Reflection;
using Gybs.DependencyInjection;
using Gybs.Internal;
using Gybs.Logic.Operations.Factory;
using Gybs.Logic.Operations.Factory.Internal;
using Gybs.Logic.Operations.ServiceProvider;
using Microsoft.Extensions.DependencyInjection;

namespace Gybs.Logic.Operations
{
    /// <summary>
    /// <see cref="GybsServicesBuilder"/> extensions.
    /// </summary>
    public static class GybsServicesBuilderExtensions
    {
        /// <summary>
        /// Adds a <see cref="IOperationBus"/> which handles operations by resolving a handler from service provider.
        /// </summary>
        /// <param name="servicesBuilder">The builder.</param>
        /// <returns>The builder.</returns>
        public static GybsServicesBuilder AddServiceProviderOperationBus(this GybsServicesBuilder servicesBuilder)
        {
            ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance
                .AddScoped<IOperationBus, ServiceProviderOperationBus>();
            return servicesBuilder;
        }

        /// <summary>
        /// Adds a <see cref="IOperationFactory"/> which allows to create and handle operations via <see cref="IOperationBus"/>.
        /// </summary>
        /// <param name="servicesBuilder">The builder.</param>
        /// <returns>The builder.</returns>
        public static GybsServicesBuilder AddOperationFactory(this GybsServicesBuilder servicesBuilder)
        {
            ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance
                .AddScoped<IOperationFactory, OperationFactory>();
            return servicesBuilder;
        }

        /// <summary>
        /// Adds all operation initializers from the assembly which are used by factory to initialize operations.
        /// </summary>
        /// <param name="servicesBuilder">The builder.</param>
        /// <param name="assembly">The assembly. If not provided, <see cref="Assembly.GetCallingAssembly"/> is used.</param>
        /// <returns>The builder.</returns>
        public static GybsServicesBuilder AddOperationInitializersForFactory(this GybsServicesBuilder servicesBuilder, Assembly assembly = null)
        {
            ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance
                .AddTypesImplementingInterfaceFromAssembly(typeof(IOperationInitializer), assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Scoped);
            return servicesBuilder;
        }

        /// <summary>
        /// Adds all operation handlers from the assembly.
        /// </summary>
        /// <param name="servicesBuilder">The builder.</param>
        /// <param name="assembly">The assembly. If not provided, <see cref="Assembly.GetCallingAssembly"/> is used.</param>
        /// <returns>The builder.</returns>
        public static GybsServicesBuilder AddOperationHandlers(this GybsServicesBuilder servicesBuilder, Assembly assembly = null)
        {
            var serviceCollection = ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance;
            serviceCollection.AddTypesImplementingInterfaceFromAssembly(typeof(IOperationHandler<>), assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Transient);
            serviceCollection.AddTypesImplementingInterfaceFromAssembly(typeof(IOperationHandler<,>), assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Transient);

            return servicesBuilder;
        }
    }
}
