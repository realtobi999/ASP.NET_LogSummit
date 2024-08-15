using System.Text.Json.Serialization;
using LogSummitApi.Application.Core.Utilities;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Presentation.Extensions;
using LogSummitApi.Presentation.Middleware.Filters;
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
            builder.Services.ConfigureMappers();

            builder.Services.AddSingleton<IHasher, Hasher>();

            builder.Services.ConfigureJwtAuthentication(config);

            // user authorization
            builder.Services.AddAuthorizationBuilder().AddPolicy("User", policy => policy.RequireRole("User"));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<CustomDtoSerializationFilter>(1);
                options.Filters.Add<CustomSuccessSerializationFilter>(2);
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
        }

        var app = builder.Build();
        {
            app.UseExceptionHandler(opt => { });

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