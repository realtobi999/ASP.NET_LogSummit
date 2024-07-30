using LogSummitApi.Application.Core.Factories;
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

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();

            builder.Services.AddExceptionHandler<ExceptionHandler>();

            builder.Services.ConfigureDbContext(config);
            builder.Services.ConfigureRepositoryManager();

            builder.Services.ConfigureJwtAuthentication(config);
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