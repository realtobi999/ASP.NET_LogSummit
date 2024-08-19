using LogSummitApi.Domain.Core.Interfaces.Repositories;

namespace LogSummitApi.Domain.Core.Interfaces.Factories;

public interface IRepositoryFactory
{
    IUserRepository CreateUserRepository();
    IRouteAttemptRepository CreateRouteAttemptRepository();
    ISummitRepository CreateSummitRepository();
    IRouteRepository CreateRouteRepository();
}