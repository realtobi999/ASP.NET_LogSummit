namespace LogSummitApi.Domain.Core.Interfaces.Services;

public interface IServiceManager
{
    public ISummitService Summit { get; }
    public IRouteService Route { get; }
    public IUserService User { get; }
}
