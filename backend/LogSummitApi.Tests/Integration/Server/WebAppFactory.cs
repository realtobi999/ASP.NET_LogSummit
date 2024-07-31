using LogSummitApi.Infrastructure.Persistance;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LogSummitApi.Tests.Integration.Server;

public class WebAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private readonly string _dbName = Guid.NewGuid().ToString();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.ReplaceWithInMemoryDatabase<LogSummitContext>(_dbName);
        });
    }
}
