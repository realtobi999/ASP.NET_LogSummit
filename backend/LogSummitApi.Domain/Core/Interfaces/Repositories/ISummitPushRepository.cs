using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Repositories;

public interface IRouteRepository : IBaseRepository<Route>
{
    Task<Route?> GetAsync(Guid id);
}
