using LogSummitApi.Application.Core.Validators;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Factories;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Utilities;

namespace LogSummitApi.Application.Core.Factories;

public class ValidatorFactory : IValidatorFactory
{
    IRepositoryManager _repository;

    public ValidatorFactory(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public IValidator<Summit> CreateSummitValidator()
    {
        return new SummitValidator(_repository);
    }
}
