using LogSummitApi.Domain.Core.Dto.Summits.Routes.Attempts;
using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Mappers;

public interface IRouteAttemptMapper : IBaseMapper<RouteAttempt, CreateRouteAttemptDto, UpdateRouteAttemptDto>
{
}
