using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Utilities;

namespace LogSummitApi.Domain.Core.Interfaces.Factories;

public interface IValidatorFactory
{
    IValidator<Summit> CreateSummitValidator();
    IValidator<Route> CreateRouteValidator();
}
