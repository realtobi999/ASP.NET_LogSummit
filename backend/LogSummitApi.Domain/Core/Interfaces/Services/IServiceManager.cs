namespace LogSummitApi.Domain.Core.Interfaces.Services;

public interface IServiceManager
{
    public ISummitService Summit { get; }
    public ISummitPushService SummitPushes { get; }
    public IUserService Users { get; }
}
