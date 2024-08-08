namespace LogSummitApi.Domain.Core.Interfaces.Common;

public interface IValidator<T>
{
    Task<(bool isValid, Exception? exception)> IsValidAsync(T entity);
}
