using GeoCoordinates.Core.Extensions;
using LogSummitApi.Domain.Core.Dto.Summits.Routes;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Common;
using LogSummitApi.Domain.Core.Interfaces.Mappers;

namespace LogSummitApi.Application.Core.Mappers;

public class RouteMapper : IRouteMapper
{
    public Route CreateEntityFromDto(CreateRouteDto dto)
    {
        var coordinates = dto.Coordinates ?? throw new NullPropertyException(nameof(CreateRouteDto), nameof(CreateRouteDto.Coordinates));

        return new Route()
        {
            Id = dto.Id ?? Guid.NewGuid(),
            SummitId = dto.SummitId,
            UserId = dto.UserId,
            Name = dto.Name,
            Description = dto.Description,
            Distance = coordinates.GetDistance(),
            ElevationGain = coordinates.GetElevationGain(),
            ElevationLoss = coordinates.GetElevationLoss(),
            IsPublic = dto.IsPublic ?? throw new NullPropertyException(nameof(CreateRouteDto), nameof(CreateRouteDto.IsPublic)),
            Coordinates = coordinates,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateEntityFromDto(Route route, UpdateRouteDto dto)
    {
        route.Name = dto.Name;
        route.Description = dto.Description;
        route.IsPublic = dto.IsPublic ?? throw new NullPropertyException(nameof(UpdateRouteDto), nameof(UpdateRouteDto.IsPublic));
        route.Coordinates = dto.Coordinates;
    }
}
