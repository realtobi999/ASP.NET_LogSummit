using LogSummitApi.Domain.Core.Dto.Summits.Routes;
using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Mappers;

public interface IRouteMapper : IBaseMapper<Route, CreateRouteDto, UpdateRouteDto>
{
}
