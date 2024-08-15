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

    public async Task<IEnumerable<Route>> IndexAsync()
    {
        var routes = await _repository.Route.IndexAsync();

        return routes.OrderBy(sp => sp.CreatedAt);
    }

    public async Task<Route> GetAsync(Guid id)
    {
        var route = await _repository.Route.GetAsync(id) ?? throw new NotFound404Exception(nameof(Route), id);

        return route;
    }

    public async Task CreateAsync(Route route)
    {
        // validate the object
        var (valid, exception) = await _validator.IsValidAsync(route);
        if (!valid && exception is not null) throw exception;

        _repository.Route.Create(route);

        await _repository.SaveSafelyAsync();
    }

    public async Task UpdateAsync(Route route)
    {
        // validate the object
        var (valid, exception) = await _validator.IsValidAsync(route);
        if (!valid && exception is not null) throw exception;

        _repository.Route.Update(route);

        await _repository.SaveSafelyAsync();
    }

    public async Task DeleteAsync(Route route)
    {
        _repository.Route.Delete(route);

        await _repository.SaveSafelyAsync();
    }
}
