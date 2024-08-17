using LogSummitApi.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogSummitApi.Infrastructure.Persistance.Extensions;

public static class LogSummitContextExtensions
{
    /// <summary>
    /// Configures the relationship between a User and a Summit.
    /// </summary>
    /// <param name="builder">The ModelBuilder instance to configure the relationship on.</param>
    /// <remarks>
    /// This method configures a one-to-many relationship between a User and a Summit, 
    /// where a User can have multiple Summits and a Summit is associated with one User.
    /// </remarks>
    public static void ConfigureUserToSummitRelationship(this ModelBuilder builder)
    {
        builder.Entity<Summit>()
               .HasOne(s => s.User)
               .WithMany(u => u.Summits)
               .HasForeignKey(s => s.UserId);
    }

    /// <summary>
    /// Configures the relationship between a User and a Route.
    /// </summary>
    /// <param name="builder">The ModelBuilder instance to configure the relationship on.</param>
    /// <remarks>
    /// This method configures a one-to-many relationship between a User and a Route, 
    /// where a User can have multiple Routes and a Route is associated with one User.
    /// </remarks>
    public static void ConfigureUserToRouteRelationship(this ModelBuilder builder)
    {
        builder.Entity<Route>()
               .HasOne(r => r.User)
               .WithMany(u => u.Routes)
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configures the relationship between a Summit and a Route.
    /// </summary>
    /// <param name="builder">The ModelBuilder instance to configure the relationship on.</param>
    /// <remarks>
    /// This method configures a one-to-many relationship between a Summit and a Route, 
    /// where a Summit can have multiple Routes and a Route is associated with one Summit.
    /// </remarks>
    public static void ConfigureSummitToRouteRelationship(this ModelBuilder builder)
    {
        builder.Entity<Route>()
               .HasOne(r => r.Summit)
               .WithMany(s => s.Routes)
               .HasForeignKey(r => r.SummitId)
               .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configures the relationship between a Route and a RouteAttempt.
    /// </summary>
    /// <param name="builder">The ModelBuilder instance to configure the relationship on.</param>
    /// <remarks>
    /// This method configures a one-to-many relationship between a Route and a
    /// <summary>
    public static void ConfigureRouteToRouteAttemptRelationship(this ModelBuilder builder)
    {
        builder.Entity<RouteAttempt>()
               .HasOne(ra => ra.Route)
               .WithMany(r => r.RouteAttempts)
               .HasForeignKey(ra => ra.RouteId)
               .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configures the relationship between a User and a RouteAttempt.
    /// </summary>
    /// <param name="builder">The ModelBuilder instance to configure the relationship on.</param>
    /// <remarks>
    /// This method configures a one-to-many relationship between a User and a RouteAttempt, 
    /// where a User can have multiple RouteAttempts and a RouteAttempt is associated with one User.
    /// </remarks>
    public static void ConfigureUserToRouteAttemptRelationship(this ModelBuilder builder)
    {
        builder.Entity<RouteAttempt>()
               .HasOne(ra => ra.User)
               .WithMany(u => u.RouteAttempts)
               .HasForeignKey(ra => ra.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}