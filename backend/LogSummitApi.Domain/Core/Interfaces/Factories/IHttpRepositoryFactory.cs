
using LogSummitApi.Domain.Core.Interfaces.Repositories.HTTP;

namespace LogSummitApi.Domain.Core.Interfaces.Factories;

public interface IHttpRepositoryFactory
{
    IHttpCountryRepository CreateCountryHttpRepository();
}
