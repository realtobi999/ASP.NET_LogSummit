using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Repositories.HTTP;

namespace LogSummitApi.Domain.Core.Interfaces.Factories;

public interface IRepositoryFactory
{
    IUserRepository CreateUserRepository();
    ISummitRepository CreateSummitRepository();
    IHttpCountryRepository CreateHttpCountryRepository();
}