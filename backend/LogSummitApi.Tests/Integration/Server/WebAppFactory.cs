using LogSummitApi.Infrastructure.Persistance;
using LogSummitApi.Presentation.Middleware.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace LogSummitApi.Tests.Integration.Server;

public class WebAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private readonly string _dbName = Guid.NewGuid().ToString();
    private static readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions()); // we use static property to make sure every test instance uses the same cache
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.ReplaceWithInMemoryDatabase<LogSummitContext>(_dbName);

            services.RemoveService<MemoryCache>();
            services.RemoveFilter<CustomSuccessSerializationFilter>();
            services.AddSingleton(_cache);
        });
    }
}
