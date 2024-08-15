using System.Text;
using LogSummitApi.Application.Core.Factories;
using LogSummitApi.Application.Core.Mappers;
using LogSummitApi.Application.Core.Services;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Interfaces.Factories;
using LogSummitApi.Domain.Core.Interfaces.Mappers;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Services;
using LogSummitApi.Infrastructure.Factories;
using LogSummitApi.Infrastructure.Persistance;
using LogSummitApi.Infrastructure.Persistance.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LogSummitApi.Presentation.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var jwt = JwtFactory.Create(config.GetSection("JWT:Issuer").Value, config.GetSection("JWT:Key").Value);

        services.AddSingleton<IJwt>(_ => jwt);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwt.Issuer,
                        ValidAudience = jwt.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key))
                    };
                }
            );
    }

    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LogSummitContext>(options =>
       {
           options.UseNpgsql(configuration.GetConnectionString("LogSummit"));
       });
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.AddScoped<IHttpRepositoryFactory, HttpRepositoryFactory>();
        services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void ConfigureServiceManager(this IServiceCollection services)
    {
        services.AddScoped<IServiceFactory, ServiceFactory>();
        services.AddScoped<IServiceManager, ServiceManager>();
    }

    public static void ConfigureValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidatorFactory, ValidatorFactory>();
    }

    public static void ConfigureMappers(this IServiceCollection services)
    {
        services.AddSingleton<IUserMapper, UserMapper>();
        services.AddSingleton<ISummitMapper, SummitMapper>();
        services.AddSingleton<IRouteMapper, RouteMapper>();
    }
}
