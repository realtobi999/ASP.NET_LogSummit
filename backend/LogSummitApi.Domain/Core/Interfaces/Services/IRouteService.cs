using LogSummitApi.Domain.Core.Dto.Summits.Routes;
using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Services;

public interface IRouteService
{
    Task<IEnumerable<Route>> IndexAsync();
    Task<Route> GetAsync(Guid id);
    Task CreateAsync(Route route);
    Task UpdateAsync(Route route);
    Task DeleteAsync(Route route);
}
