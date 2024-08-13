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
    /// Configures one-to-many relationship between <see cref="User"/> and <see cref="Route"/> .
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> used to configure the entity relationships.</param>
    public static void ConfigureUserToRouteRelationship(this ModelBuilder builder)
    {
        builder.Entity<Route>()
               .HasOne(sp => sp.User)
               .WithMany(u => u.Routes)
               .HasForeignKey(sp => sp.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configures one-to-many relationship between <see cref="Summit"/> and <see cref="Route"/> .
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> used to configure the entity relationships.</param>
    public static void ConfigureSummitToRouteRelationship(this ModelBuilder builder)
    {
        builder.Entity<Route>()
               .HasOne(sp => sp.Summit)
               .WithMany(s => s.Routes)
               .HasForeignKey(sp => sp.SummitId)
               .OnDelete(DeleteBehavior.Cascade); 
    }
}
