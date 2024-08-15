using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Services;

public interface ISummitService
{
    Task<IEnumerable<Summit>> IndexAsync();
    Task<Summit> GetAsync(Guid id);
    Task<Summit> CreateAsync(CreateSummitDto createSummitDto);
    Task UpdateAsync(Summit summit, UpdateSummitDto updateSummitDto);
    Task DeleteAsync(Summit summit);
    Task<IEnumerable<string>> GetValidCountriesAsync();
}
