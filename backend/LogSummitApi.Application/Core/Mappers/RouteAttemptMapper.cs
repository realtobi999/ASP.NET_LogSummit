using LogSummitApi.Domain.Core.Dto.Summits.Routes.Attempts;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Common;
using LogSummitApi.Domain.Core.Interfaces.Mappers;

namespace LogSummitApi.Application.Core.Mappers;

public class RouteAttemptMapper : IRouteAttemptMapper
{
    public RouteAttempt CreateEntityFromDto(CreateRouteAttemptDto dto)
    {
        return new RouteAttempt
        {
            Id = dto.Id ?? Guid.NewGuid(),
            UserId = dto.UserId,
            SummitId = dto.SummitId,
            RouteId = dto.RouteId,
            Name = dto.Name,
            Description = dto.Description,
            IsPublic = dto.IsPublic ?? throw new NullPropertyException(nameof(CreateRouteAttemptDto), nameof(CreateRouteAttemptDto.IsPublic)),
            Time = dto.Time,
            Coordinates = dto.Coordinates,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public void UpdateEntityFromDto(RouteAttempt attempt, UpdateRouteAttemptDto dto)
    {
        attempt.Name = dto.Name;
        attempt.Description = dto.Description;
        attempt.Coordinates = dto.Coordinates;
        attempt.IsPublic = dto.IsPublic ?? throw new NullPropertyException(nameof(UpdateRouteAttemptDto), nameof(UpdateRouteAttemptDto.IsPublic));
        attempt.Time = dto.Time;
    }
}
