﻿using LogSummitApi.Domain.Core.Dto.Summit.Pushes;
using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Services;

public interface ISummitPushService
{
    Task<IEnumerable<SummitPush>> IndexAsync();
    Task<SummitPush> CreateAsync(CreateSummitPushDto createSummitPushDto);
}
