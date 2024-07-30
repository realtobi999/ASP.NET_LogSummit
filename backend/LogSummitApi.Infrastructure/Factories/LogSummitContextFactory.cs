using LogSummitApi.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LogSummitApi.Infrastructure.Factories;

public class LogSummitContextFactory : IDesignTimeDbContextFactory<LogSummitContext>
{
    public LogSummitContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory() + "../../LogSummitApi.Presentation")
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();
              
        var builder = new DbContextOptionsBuilder<LogSummitContext>().UseNpgsql(configuration.GetConnectionString("LogSummit"));

        return new LogSummitContext(builder.Options);
    }
}
