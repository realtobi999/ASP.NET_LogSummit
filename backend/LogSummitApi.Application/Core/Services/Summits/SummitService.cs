using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.HTTP;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Services;

namespace LogSummitApi.Application.Core.Services.Summits;

public class SummitService : ISummitService
{
    private readonly IRepositoryManager _repository;

    public SummitService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<Summit> Create(CreateSummitDto createSummitDto)
    {
        // check if the user exists in the database
        var user = await _repository.Users.Get(createSummitDto.UserId) ?? throw new NotFound404Exception(nameof(User), createSummitDto.UserId);

        var summit = new Summit()
        {
            Id = createSummitDto.Id ?? Guid.NewGuid(),
            UserId = user.Id,
            Name = createSummitDto.Name,
            Description = createSummitDto.Description,
            CreatedAt = createSummitDto.CreatedAt,
            Coordinate = createSummitDto.Coordinate, 
        };

        _repository.Summit.Create(summit);
        await _repository.SaveSafelyAsync();

        return summit;
    }
}
