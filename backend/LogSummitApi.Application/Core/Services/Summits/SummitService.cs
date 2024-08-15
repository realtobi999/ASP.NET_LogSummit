using LogSummitApi.Domain.Core.Dto.Summits;
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

    public async Task<IEnumerable<Summit>> IndexAsync()
    {
        var summits = await _repository.Summit.IndexAsync();

        return summits;
    }
    
    public async Task<Summit> GetAsync(Guid id)
    {
        var summit = await _repository.Summit.GetAsync(id) ?? throw new NotFound404Exception(nameof(Summit), id);

        return summit;
    }

    public async Task CreateAsync(Summit summit)
    {
        // validate the object
        var (valid, exception) = await _validator.IsValidAsync(summit);
        if (!valid && exception is not null) throw exception;

        _repository.Summit.Create(summit);
        
        await _repository.SaveSafelyAsync();
    }

    public async Task UpdateAsync(Summit summit)
    {
        // validate the object
        var (valid, exception) = await _validator.IsValidAsync(summit);
        if (!valid && exception is not null) throw exception;

        _repository.Summit.Update(summit);

        await _repository.SaveSafelyAsync();
    }
    
    public async Task DeleteAsync(Summit summit)
    {
        _repository.Summit.Delete(summit);

        await _repository.SaveSafelyAsync();
    }

    public async Task<IEnumerable<string>> GetValidCountriesAsync()
    {
        var countries = await _repository.HttpCountry.IndexAsync();

        return countries.Select(country =>
        {
            if (country.Name == null) throw new NullPropertyException(nameof(Country), nameof(Country.Name));
            if (country.Name.Common == null) throw new NullPropertyException(nameof(Country), nameof(Country.Name.Common));

            return country.Name.Common;
        }).ToList();
    }
}
