namespace LogSummitApi.Domain.Core.Interfaces.Services;

public interface IServiceManager
{
    public ISummitService Summit { get; }
    public ISummitPushService SummitPush { get; }
    public IUserService User { get; }
}
