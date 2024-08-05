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
    /// Configures one-to-many relationship between <see cref="SummitPush"/> and <see cref="User"/> .
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> used to configure the entity relationships.</param>
    public static void ConfigureSummitPushToUserRelationShip(this ModelBuilder builder)
    {
        builder.Entity<SummitPush>()
               .HasOne(sp => sp.User)
               .WithMany(u => u.SummitPushes)
               .HasForeignKey(sp => sp.UserId);
    }

    /// <summary>
    /// Configures one-to-many relationship between <see cref="SummitPush"/> and <see cref="Summit"/> .
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> used to configure the entity relationships.</param>
    public static void ConfigureSummitPushToSummitRelationship(this ModelBuilder builder)
    {
        builder.Entity<SummitPush>()
               .HasOne(sp => sp.Summit)
               .WithMany(s => s.SummitPushes)
               .HasForeignKey(sp => sp.SummitId); // Note: I changed UserId to SummitId, assuming that's the correct foreign key
    }
}
