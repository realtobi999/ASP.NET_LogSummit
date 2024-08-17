using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Infrastructure.Persistance.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LogSummitApi.Infrastructure.Persistance;

public class LogSummitContext(DbContextOptions<LogSummitContext> opt) : DbContext(opt)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // configure specific properties
        builder.Entity<User>()
               .HasIndex(u => u.Email)
               .IsUnique();

        // configure entity relationships      
        builder.ConfigureUserToRouteAttemptRelationship();
        builder.ConfigureUserToRouteRelationship();
        builder.ConfigureUserToSummitRelationship();

        builder.ConfigureRouteToRouteAttemptRelationship();

        builder.ConfigureSummitToRouteRelationship();
    }
}
