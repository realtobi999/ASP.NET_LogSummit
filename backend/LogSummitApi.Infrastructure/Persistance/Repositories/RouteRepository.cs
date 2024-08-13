using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LogSummitApi.Infrastructure.Persistance.Repositories;

public class RouteRepository : BaseRepository<Route>, IRouteRepository
{
    public RouteRepository(LogSummitContext context) : base(context)
    {
    }

    protected override IQueryable<Route> GetQueryable()
    {
        return base.GetQueryable()
                   .Include(r => r.User)
                   .Include(r => r.Summit);
    }

    public async Task<Route?> GetAsync(Guid id)
    {
        return await this.GetAsync(sp => sp.Id == id);
    }
}
