namespace LogSummitApi.Domain.Core.Interfaces.Common;

/// <summary>
/// Defines a contract for validating entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of entity that will be validated.</typeparam>
public interface IValidator<T>
{
    /// <summary>
    /// Asynchronously validates the specified entity.
    /// </summary>
    /// <param name="entity">The entity to be validated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple where:
    ///     - <c>isValid</c> indicates whether the entity is valid (<c>true</c>) or not (<c>false</c>).
    ///     - <c>exception</c> contains an exception that describes the validation issue if the entity is not valid; otherwise, <c>null</c>.</returns>
    Task<(bool isValid, Exception? exception)> IsValidAsync(T entity);
}