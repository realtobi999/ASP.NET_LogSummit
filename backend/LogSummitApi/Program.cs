using LogSummitApi.Application.Core.Factories;
using LogSummitApi.Domain.Core.Interfaces.Utilities;
using LogSummitApi.Presentation.Extensions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            var config = builder.Configuration;

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();

            // JWT 
            var jwt = JwtFactory.Create(config.GetSection("JWT:Issuer").Value, config.GetSection("JWT:Key").Value);
            builder.Services.AddSingleton<IJwt>(_ => jwt);
            builder.Services.ConfigureJwtAuthentication(jwt);
        }

        var app = builder.Build();
        {
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