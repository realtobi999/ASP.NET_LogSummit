namespace LogSummitApi.Domain.Core.Interfaces.Mappers;

public interface IBaseMapper<Entity, CreateEntityDto, UpdateEntityDto>
{
    public Entity CreateEntityFromDto(CreateEntityDto dto);
    public void UpdateEntityFromDto(Entity entity, UpdateEntityDto dto);
}
