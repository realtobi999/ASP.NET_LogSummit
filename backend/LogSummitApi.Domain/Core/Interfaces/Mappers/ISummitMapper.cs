using LogSummitApi.Domain.Core.Dto.Summits;
using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Mappers;

public interface ISummitMapper : IBaseMapper<Summit, CreateSummitDto, UpdateSummitDto>
{
}
