﻿using LogSummitApi.Domain.Core.Interfaces.Services;

namespace LogSummitApi.Domain.Core.Interfaces.Factories;

public interface IServiceFactory
{
    IUserService CreateUserService();
    ISummitService CreateSummitService();
    ISummitPushService CreateSummitPushService();
}
