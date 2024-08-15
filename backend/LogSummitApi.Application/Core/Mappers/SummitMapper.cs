using LogSummitApi.Domain.Core.Dto.Summits;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Common;
using LogSummitApi.Domain.Core.Interfaces.Mappers;

namespace LogSummitApi.Application.Core.Mappers;

public class SummitMapper : ISummitMapper
{
    public Summit CreateEntityFromDto(CreateSummitDto dto)
    {
        return new Summit
        {
            Id = dto.Id ?? Guid.NewGuid(),
            UserId = dto.UserId,
            Name = dto.Name,
            Description = dto.Description,
            Country = dto.Country,
            IsPublic = dto.IsPublic ?? throw new NullPropertyException(nameof(CreateSummitDto), nameof(CreateSummitDto.IsPublic)),
            CreatedAt = DateTime.UtcNow,
            Coordinate = dto.Coordinate
        };
    }

    public void UpdateEntityFromDto(Summit summit, UpdateSummitDto dto)
    {
        summit.Name = dto.Name;
        summit.Description = dto.Description;
        summit.Country = dto.Country;
        summit.IsPublic = dto.IsPublic ?? throw new NullPropertyException(nameof(CreateSummitDto), nameof(CreateSummitDto.IsPublic));
        summit.Coordinate = dto.Coordinate;
    }
}
