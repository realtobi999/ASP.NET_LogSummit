using LogSummitApi.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogSummitApi.Infrastructure.Persistance;

public class LogSummitContext(DbContextOptions<LogSummitContext> opt) : DbContext(opt)
{
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
    }
}
