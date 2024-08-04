namespace LogSummitApi.Domain.Core.Interfaces.Utilities;

public interface IValidator<T>
{
    Task<(bool isValid, Exception? exception)> IsValidAsync(T entity);
}
