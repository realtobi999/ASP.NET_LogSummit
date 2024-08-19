using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Repositories;

public interface IRouteAttemptRepository : IBaseRepository<RouteAttempt>
{
    Task<RouteAttempt?> GetAsync(Guid id);
}