
using LogSummitApi.Domain.Core.Interfaces.Repositories.Http;

namespace LogSummitApi.Domain.Core.Interfaces.Factories;

public interface IHttpRepositoryFactory
{
    IHttpCountryRepository CreateCountryHttpRepository();
}
