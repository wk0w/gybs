﻿using System.Reflection;
using Gybs.DependencyInjection;
using Gybs.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Gybs.Data.Repositories
{
    /// <summary>
    /// <see cref="GybsServicesBuilder"/> extensions.
    /// </summary>
    public static class GybsServicesBuilderExtensions
    {
        /// <summary>
        /// Adds <see cref="IUnitOfWork"/> implementations from the assembly.
        /// </summary>
        /// <param name="servicesBuilder">The builder.</param>
        /// <param name="serviceLifetime">The lifetime of registered service.</param>
        /// <param name="assembly">The assembly. If not provided, <see cref="Assembly.GetCallingAssembly"/> is used.</param>
        /// <returns>The builder.</returns>
        public static GybsServicesBuilder AddUnitOfWork(this GybsServicesBuilder servicesBuilder, ServiceLifetime serviceLifetime, Assembly? assembly = null)
        {
            ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance
                .AddTypesImplementingInterfaceFromAssembly(typeof(IUnitOfWork), assembly ?? Assembly.GetCallingAssembly(), serviceLifetime);
            return servicesBuilder;
        }

        /// <summary>
        /// Adds <see cref="IRepository{TEntity}"/> implementations from the assembly.
        /// </summary>
        /// <param name="servicesBuilder">The builder.</param>
        /// <param name="serviceLifetime">The lifetime of registered service.</param>
        /// <param name="assembly">The assembly. If not provided, <see cref="Assembly.GetCallingAssembly"/> is used.</param>
        /// <returns>The builder.</returns>
        public static GybsServicesBuilder AddRepositories(this GybsServicesBuilder servicesBuilder, ServiceLifetime serviceLifetime, Assembly? assembly = null)
        {
            ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance
                .AddTypesImplementingInterfaceFromAssembly(typeof(IRepository<>), assembly ?? Assembly.GetCallingAssembly(), serviceLifetime);
            return servicesBuilder;
        }
    }
}
