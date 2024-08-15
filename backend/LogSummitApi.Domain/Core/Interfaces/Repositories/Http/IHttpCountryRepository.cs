using LogSummitApi.Domain.Core.Dto.Summits;

namespace LogSummitApi.Domain.Core.Interfaces.Repositories.Http;

public interface IHttpCountryRepository
{
    Task<IEnumerable<Country>> IndexAsync();
}
