using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Common;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Services;

namespace LogSummitApi.Application.Core.Services.Summits;

public class SummitService : ISummitService
{
    private readonly IRepositoryManager _repository;
    private readonly IValidator<Summit> _validator;

    public SummitService(IRepositoryManager repository, IValidator<Summit> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Summit> CreateAsync(CreateSummitDto createSummitDto)
    {

        var summit = new Summit
        {
            Id = createSummitDto.Id ?? Guid.NewGuid(),
            UserId = createSummitDto.UserId,
            Name = createSummitDto.Name,
            Description = createSummitDto.Description,
            Country = createSummitDto.Country,
            IsPublic = createSummitDto.IsPublic ?? throw new NullPropertyException(nameof(Summit), nameof(Summit.IsPublic)),
            CreatedAt = DateTime.UtcNow,
            Coordinate = createSummitDto.Coordinate
        };

        // validate the object
        var (valid, exception) = await _validator.IsValidAsync(summit);
        if (!valid && exception is not null) throw exception;

        _repository.Summit.Create(summit);
        await _repository.SaveSafelyAsync();

        return summit;
    }

    public async Task DeleteAsync(Summit summit)
    {
        _repository.Summit.Delete(summit);
        
        await _repository.SaveSafelyAsync();
    }

    public async Task<Summit> GetAsync(Guid id)
    {
        var summit = await _repository.Summit.GetAsync(id) ?? throw new NotFound404Exception(nameof(Summit), id);

        return summit;
    }

    public async Task<IEnumerable<string>> GetValidCountriesAsync()
    {
        var countries = await _repository.HttpCountry.IndexAsync();

        return countries
            .Select(country =>
            {
                if (country.Name == null) throw new NullPropertyException(nameof(CountryDto), nameof(CountryDto.Name));
                if (country.Name.Common == null) throw new NullPropertyException(nameof(CountryDto), nameof(CountryDto.Name.Common));

                return country.Name.Common;
            })
            .ToList();
    }

    public async Task<IEnumerable<Summit>> IndexAsync()
    {
        var summits = await _repository.Summit.IndexAsync();

        return summits.OrderBy(s => s.CreatedAt);
    }

    public async Task UpdateAsync(Summit summit, UpdateSummitDto updateSummitDto)
    {
        summit.Name = updateSummitDto.Name;
        summit.Description = updateSummitDto.Description;
        summit.Country = updateSummitDto.Country;

        // validate the object
        var (valid, exception) = await _validator.IsValidAsync(summit);
        if (!valid && exception is not null) throw exception;

        await _repository.SaveSafelyAsync();
    }
}
