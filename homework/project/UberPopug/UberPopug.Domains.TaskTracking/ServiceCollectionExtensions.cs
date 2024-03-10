using Microsoft.Extensions.DependencyInjection;

namespace UberPopug.Domains.TaskTracking;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTaskTracking<T>(this IServiceCollection services)
        where T: class, ITaskTrackingDatabaseContext
    {
        return services.AddTransient<ITaskTrackingDatabaseContext, T>();
    }
}