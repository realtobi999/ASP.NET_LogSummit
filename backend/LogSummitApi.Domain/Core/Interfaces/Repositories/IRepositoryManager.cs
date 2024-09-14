using LogSummitApi.Domain.Core.Interfaces.Repositories.Http;

namespace LogSummitApi.Domain.Core.Interfaces.Repositories;

public interface IRepositoryManager
{
    public IUserRepository Users { get; }
    public ISummitRepository Summit { get; }
    public IRouteRepository Route { get; }
    public IRouteAttemptRepository RouteAttempt { get; }
    public IHttpCountryRepository HttpCountry { get; }
    public Task<int> SaveAsync();

    /// <summary>
    /// Performs a check of how many rows were affected, if zero throws an <see cref="ZeroRowsAffectedException"/>.
    /// </summary>
    public Task SaveSafelyAsync();
}
