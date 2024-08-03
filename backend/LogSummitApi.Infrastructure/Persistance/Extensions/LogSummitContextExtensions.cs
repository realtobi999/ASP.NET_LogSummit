using LogSummitApi.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogSummitApi.Infrastructure.Persistance.Extensions;

public static class LogSummitContextExtensions
{
    /// <summary>
    /// Configures one-to-many relationship between <c>Summit</c> and <c>User</c>.
    /// </summary>
    /// <param name="builder">The <see cref="ModelBuilder"/> used to configure the entity relationships.</param>
    public static void ConfigureSummitUserRelationship(this ModelBuilder builder)
    {
        builder.Entity<Summit>()
               .HasOne(s => s.User)
               .WithMany(u => u.Summits)
               .HasForeignKey(s => s.UserId);
    }
}
