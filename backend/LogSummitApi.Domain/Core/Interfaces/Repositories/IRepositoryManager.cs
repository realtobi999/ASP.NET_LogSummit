namespace LogSummitApi.Domain.Core.Interfaces.Repositories;

public interface IRepositoryManager
{
    public Task<int> SaveAsync();
}
