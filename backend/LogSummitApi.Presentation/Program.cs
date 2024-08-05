using LogSummitApi.Application.Core.Utilities;
using LogSummitApi.Domain.Core.Interfaces.Utilities;
using LogSummitApi.Presentation.Extensions;
using LogSummitApi.Presentation.Middleware.Handlers;

namespace LogSummitApi.Presentation;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            var config = builder.Configuration;

            builder.Services.AddExceptionHandler<ExceptionHandler>();

            builder.Services.ConfigureDbContext(config);
            builder.Services.AddHttpClient();
            builder.Services.AddMemoryCache();

            // services - dependency injection
            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();
            builder.Services.ConfigureValidators();
            
            builder.Services.AddSingleton<IHasher, Hasher>();

            builder.Services.ConfigureJwtAuthentication(config);

            // user authorization
            builder.Services.AddAuthorizationBuilder().AddPolicy("User", policy => policy.RequireRole("User"));
                                                
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();

        }

        var app = builder.Build();
        {
            app.UseExceptionHandler(opt => {});

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}