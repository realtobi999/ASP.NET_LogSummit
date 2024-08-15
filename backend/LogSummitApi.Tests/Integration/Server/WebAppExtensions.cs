using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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

    public static void RemoveFilter<TFilter>(this IServiceCollection services) where TFilter : IFilterMetadata
    {
        services.Configure<MvcOptions>(options =>
        {
            var filterToRemove = options.Filters
                .FirstOrDefault(f => f is TypeFilterAttribute typeFilter &&
                                     typeFilter.ImplementationType == typeof(TFilter));

            if (filterToRemove != null)
            {
                options.Filters.Remove(filterToRemove);
            }
        });
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
