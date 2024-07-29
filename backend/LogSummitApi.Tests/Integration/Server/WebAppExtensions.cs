using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LogSummitApi.Tests.Integration.Server;

public static class WebAppExtensions
{
    public static void RemoveService<Service>(this IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(Service));

        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
    }

    public static void ReplaceWithInMemoryDatabase<TContext>(this IServiceCollection services, string dbName) where TContext : DbContext
    {
        services.RemoveService<DbContextOptions<TContext>>();

        services.AddDbContext<TContext>(options =>
        {
            options.UseInMemoryDatabase(dbName);
        });
    }
}
