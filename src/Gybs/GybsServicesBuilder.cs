using Gybs.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Gybs;

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
