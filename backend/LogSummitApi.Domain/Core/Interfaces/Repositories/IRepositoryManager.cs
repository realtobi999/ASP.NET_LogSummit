namespace LogSummitApi.Domain.Core.Interfaces.Repositories;

public interface IRepositoryManager
{
    public IUserRepository Users { get; }
    public Task<int> SaveAsync();

    /// <summary>
    /// Performs a check of how many rows were affected, if zero throws an <see cref="ZeroRowsAffectedException"/>.
    /// </summary>
    /// <returns></returns>
    public Task SaveSafelyAsync();
}
