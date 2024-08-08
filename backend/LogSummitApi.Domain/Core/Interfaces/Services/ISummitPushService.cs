using LogSummitApi.Domain.Core.Dto.Summit.Routes;
using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Services;

public interface IRouteService
{
    Task<IEnumerable<Route>> IndexAsync();
    Task<Route> CreateAsync(CreateRouteDto createRouteDto);
}
