using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Services;

public interface ISummitService
{
    Task<Summit> Create(CreateSummitDto createSummitDto);
}
