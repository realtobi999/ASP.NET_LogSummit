using LogSummitApi.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogSummitApi.Infrastructure.Persistance.Extensions;

public static class LogSummitContextExtensions
{
    /// <summary>
    /// Configures one-to-many relationship between <see cref="Summit"/> and <see cref="User"/> .
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> used to configure the entity relationships.</param>
    public static void ConfigureSummitToUserRelationship(this ModelBuilder builder)
    {
        builder.Entity<Summit>()
               .HasOne(s => s.User)
               .WithMany(u => u.Summits)
               .HasForeignKey(s => s.UserId);
    }

    /// <summary>
    /// Configures one-to-many relationship between <see cref="Route"/> and <see cref="User"/> .
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> used to configure the entity relationships.</param>
    public static void ConfigureRouteToUserRelationShip(this ModelBuilder builder)
    {
        builder.Entity<Route>()
               .HasOne(sp => sp.User)
               .WithMany(u => u.Routes)
               .HasForeignKey(sp => sp.UserId);
    }

    /// <summary>
    /// Configures one-to-many relationship between <see cref="Route"/> and <see cref="Summit"/> .
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> used to configure the entity relationships.</param>
    public static void ConfigureRouteToSummitRelationship(this ModelBuilder builder)
    {
        builder.Entity<Route>()
               .HasOne(sp => sp.Summit)
               .WithMany(s => s.Routes)
               .HasForeignKey(sp => sp.SummitId); // Note: I changed UserId to SummitId, assuming that's the correct foreign key
    }
}
