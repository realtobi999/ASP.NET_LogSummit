using LogSummitApi.Application.Core.Factories;
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

            // services - dependency injection
            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();
            builder.Services.AddSingleton<IHasher, Hasher>();

            builder.Services.ConfigureJwtAuthentication(config);

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

            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}