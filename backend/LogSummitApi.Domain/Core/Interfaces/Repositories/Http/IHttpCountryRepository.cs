using LogSummitApi.Domain.Core.Dto.Summit;

namespace LogSummitApi.Domain.Core.Interfaces.Repositories.Http;

public interface IHttpCountryRepository
{
    Task<IEnumerable<Country>> IndexAsync();
}
