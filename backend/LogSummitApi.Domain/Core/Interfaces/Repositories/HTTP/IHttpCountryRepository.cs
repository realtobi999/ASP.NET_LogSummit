using LogSummitApi.Domain.Core.Dto.Summit;

namespace LogSummitApi.Domain.Core.Interfaces.Repositories.HTTP;

public interface IHttpCountryRepository
{
    Task<IEnumerable<Country>> IndexAsync();
}
