using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LogSummitApi.Infrastructure.Persistance.Repositories;

public class RouteAttemptRepository : BaseRepository<RouteAttempt>, IRouteAttemptRepository
{
    public RouteAttemptRepository(LogSummitContext context) : base(context)
    {
    }

    protected override IQueryable<RouteAttempt> GetQueryable()
    {
        return base.GetQueryable()
                   .Include(ra => ra.Route)
                        .ThenInclude(r => r!.Summit)
                   .Include(ra => ra.User);
    }

    public override async Task<IEnumerable<RouteAttempt>> IndexAsync()
    {
        var attempts = await base.IndexAsync();

        return attempts.OrderBy(ra => ra.CreatedAt);
    }

    public Task<RouteAttempt?> GetAsync(Guid id)
    {
        return this.GetAsync(ra => ra.Id == id);
    }
}
