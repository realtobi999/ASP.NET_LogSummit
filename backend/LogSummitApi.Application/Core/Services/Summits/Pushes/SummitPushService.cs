using LogSummitApi.Domain.Core.Dto.Summit.Pushes;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Services;
using LogSummitApi.Domain.Core.Interfaces.Utilities;

namespace LogSummitApi.Application.Core.Services.Summits.Pushes;

public class SummitPushService : ISummitPushService
{
    private readonly IRepositoryManager _repository;
    private readonly IValidator<SummitPush> _validator;

    public SummitPushService(IRepositoryManager repository, IValidator<SummitPush> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<SummitPush> CreateAsync(CreateSummitPushDto createSummitPushDto)
    {
        var summitPush = new SummitPush()
        {
            Id = createSummitPushDto.Id,
            SummitId = createSummitPushDto.SummitId,
            UserId = createSummitPushDto.UserId,
            Name = createSummitPushDto.Name,
            Description = createSummitPushDto.Description,
            Distance = createSummitPushDto.Distance,
            ElevationGain = createSummitPushDto.ElevationGain,
            ElevationLoss = createSummitPushDto.ElevationLoss,
            Coordinates = createSummitPushDto.Coordinates ?? throw new NullReferenceException("Coordinates must be set."),
            CreatedAt = DateTime.UtcNow
        };

        // validate the object
        var (valid, exception) = await _validator.IsValidAsync(summitPush);
        if (!valid && exception is not null) throw exception;

        _repository.SummitPush.Create(summitPush);
        await _repository.SaveSafelyAsync(); 

        return summitPush;
    }

    public async Task<IEnumerable<SummitPush>> IndexAsync()
    {
        var summitPushes = await _repository.SummitPush.IndexAsync();

        return summitPushes.OrderBy(sp => sp.CreatedAt);
    }
}
