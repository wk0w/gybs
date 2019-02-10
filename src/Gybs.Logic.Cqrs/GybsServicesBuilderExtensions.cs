using System.Reflection;
using Gybs.Internal;
using Gybs.Logic.Cqrs.Factory;
using Gybs.Logic.Cqrs.Factory.Internal;
using Gybs.Logic.Cqrs.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Gybs.Logic.Cqrs
{
    /// <summary>
    /// <see cref="GybsServicesBuilder"/> extensions.
    /// </summary>
    public static class GybsServicesBuilderExtensions
    {
        /// <summary>
        /// Adds a <see cref="IOperationBus"/> which handles operations by resolving a handler from scoped service collection.
        /// </summary>
        /// <param name="servicesBuilder">The builder.</param>
        /// <returns>The builder.</returns>
        public static GybsServicesBuilder AddSelfResolvingOperationBus(this GybsServicesBuilder servicesBuilder)
        {
            ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance
                .AddScoped<IOperationBus, SelfResolvingOperationBus>();
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
            var types = new[]
            {
                typeof(IOperationInitializer)
            };

            ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance
                .AddImplementationsFromAssembly(types, assembly ?? Assembly.GetCallingAssembly(), ServiceLifetime.Scoped);
            return servicesBuilder;
        }

        /// <summary>
        /// Adds all query handlers from the assembly.
        /// </summary>
        /// <param name="servicesBuilder">The builder.</param>        
        /// <param name="assembly">The assembly. If not provided, <see cref="Assembly.GetCallingAssembly"/> is used.</param>
        /// <returns>The builder.</returns>
        public static GybsServicesBuilder AddQueryHandlers(this GybsServicesBuilder servicesBuilder, Assembly assembly = null)
        {
            var types = new[]
            {
                typeof(IQueryHandler<,>)
            };

            ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance
                .AddImplementationsFromAssembly(types, assembly ?? Assembly.GetCallingAssembly());
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
            var types = new[]
            {
                typeof(ICommandHandler<>),
                typeof(ICommandHandler<,>)
            };

            ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance
                .AddImplementationsFromAssembly(types, assembly ?? Assembly.GetCallingAssembly());
            return servicesBuilder;
        }
    }
}
