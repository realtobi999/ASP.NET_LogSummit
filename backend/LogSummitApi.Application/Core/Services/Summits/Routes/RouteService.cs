using LogSummitApi.Domain.Core.Dto.Summit.Routes;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Services;

namespace LogSummitApi.Application.Core.Services.Summits.Routes;

public class RouteService : IRouteService
{
    private readonly IRepositoryManager _repository;
    private readonly IValidator<Route> _validator;

    public RouteService(IRepositoryManager repository, IValidator<Route> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Route> CreateAsync(CreateRouteDto createRouteDto)
    {
        var route = new Route()
        {
            Id = createRouteDto.Id,
            SummitId = createRouteDto.SummitId,
            UserId = createRouteDto.UserId,
            Name = createRouteDto.Name,
            Description = createRouteDto.Description,
            Distance = createRouteDto.Distance,
            ElevationGain = createRouteDto.ElevationGain,
            ElevationLoss = createRouteDto.ElevationLoss,
            Coordinates = createRouteDto.Coordinates ?? throw new NullReferenceException("Coordinates must be set."),
            CreatedAt = DateTime.UtcNow
        };

        // validate the object
        var (valid, exception) = await _validator.IsValidAsync(route);
        if (!valid && exception is not null) throw exception;

        _repository.Route.Create(route);
        await _repository.SaveSafelyAsync(); 

        return route;
    }

    public async Task DeleteAsync(Route route)
    {
        _repository.Route.Delete(route);

        await _repository.SaveSafelyAsync();
    }

    public async Task<Route> GetAsync(Guid id)
    {
        var route = await _repository.Route.GetAsync(id) ?? throw new NotFound404Exception(nameof(Route), id); 

        return route;
    }

    public async Task<IEnumerable<Route>> IndexAsync()
    {
        var routes = await _repository.Route.IndexAsync();

        return routes.OrderBy(sp => sp.CreatedAt);
    }

    public async Task UpdateAsync(Route route, UpdateRouteDto updateRouteDto)
    {
        route.Name = updateRouteDto.Name;
        route.Description = updateRouteDto.Description;

        // validate the object
        var (valid, exception) = await _validator.IsValidAsync(route);
        if (!valid && exception is not null) throw exception;

        await _repository.SaveSafelyAsync();
    }
}
