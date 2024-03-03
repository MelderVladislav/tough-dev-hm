using Microsoft.Extensions.DependencyInjection;

namespace UberPopug.Domains.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreDomain<T>(this IServiceCollection services)
    {
        return services;
    }
}