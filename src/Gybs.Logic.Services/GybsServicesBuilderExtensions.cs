using System.Reflection;
using Gybs.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Gybs.Logic.Services
{
    /// <summary>
    /// <see cref="GybsServicesBuilder"/> extensions.
    /// </summary>
    public static class GybsServicesBuilderExtensions
    {
        /// <summary>
        /// Adds all implementations of <see cref="IService"/> from the assembly.
        /// </summary>
        /// <param name="servicesBuilder">The builder.</param>
        /// <param name="assembly">The assembly. If not provided, <see cref="Assembly.GetCallingAssembly"/> is used.</param>
        /// <returns>The builder.</returns>
        public static GybsServicesBuilder AddServices(this GybsServicesBuilder servicesBuilder, Assembly assembly = null)
        {
            var types = new[]
            {
                typeof(IService)
            };

            ((IInfrastructure<IServiceCollection>)servicesBuilder).Instance
                .AddImplementationsFromAssembly(types, assembly ?? Assembly.GetCallingAssembly());
            return servicesBuilder;
        }
    }
}
