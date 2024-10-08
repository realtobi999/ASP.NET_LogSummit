﻿using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Common;

namespace LogSummitApi.Domain.Core.Interfaces.Factories;

public interface IValidatorFactory
{
    IValidator<User> CreateUserValidator();
    IValidator<Summit> CreateSummitValidator();
    IValidator<Route> CreateRouteValidator();
    IValidator<RouteAttempt> CreateRouteAttemptValidator();
}
