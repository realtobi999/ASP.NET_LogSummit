using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Utilities;

namespace LogSummitApi.Application.Core.Validators;

public class SummitPushValidator : IValidator<SummitPush>
{
    private readonly IRepositoryManager _repository;

    public SummitPushValidator(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<(bool isValid, Exception? exception)> IsValidAsync(SummitPush summitPush)
    {
        // validate that the user exists
        if (await _repository.Users.GetAsync(summitPush.UserId) is null)
        {
            return (false, new NotFound404Exception(nameof(User), summitPush.UserId));
        }

        // validate that the summit exists
        var summit = await _repository.Summit.GetAsync(summitPush.SummitId);
        if (summit is null)
        {
            return (false, new NotFound404Exception(nameof(Summit), summitPush.SummitId));
        }

        // validate that the last coordinate is atleast 10 meters in the range of the summit coordinate
        if (summit.Coordinate is not null && !summitPush.Coordinates.Last().IsWithinRange(summit.Coordinate, Summit.SummitPushRadius))
        {
            return (false, new BadRequest400Exception($"The last coordinate of the summit push is not within a {Summit.SummitPushRadius}-meter range of the summit coordinate."));
        }

        return (true, null);
    }
}
