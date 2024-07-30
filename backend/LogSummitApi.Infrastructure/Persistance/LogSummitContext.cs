using Microsoft.EntityFrameworkCore;

namespace LogSummitApi.Infrastructure.Persistance;

public class LogSummitContext(DbContextOptions<LogSummitContext> opt) : DbContext(opt)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
    }
}
