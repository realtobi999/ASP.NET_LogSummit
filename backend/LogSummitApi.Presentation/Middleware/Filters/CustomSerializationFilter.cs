using LogSummitApi.Domain.Core.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LogSummitApi.Presentation.Middleware.Filters;

/// <summary>
/// A result filter that transforms the result of a controller action into a DTO representation.
/// <para>
/// This filter is designed to convert the result of an action into a Data Transfer Object (DTO) format. 
/// It supports both single entities and collections of entities that implement the <c>ISerializable</c> interface.
/// </para>
/// <para>
/// The transformation process includes:
/// <list type="bullet">
///     <item><description>For a single entity: If the result is an instance of <c>ISerializable</c>, it is converted to a DTO using <c>ToDto()</c>.</description></item>
///     <item><description>For a collection of entities: If the result is an <c>IEnumerable</c> of <c>ISerializable</c> objects, each entity is converted to a DTO using <c>ToDto()</c>.</description></item>
/// </list>
/// </para>
/// <para>
/// If the result is neither a single entity nor a collection of entities implementing <c>ISerializable</c>, 
/// the result remains unchanged.
/// </para>
/// </summary>
public class CustomDtoSerializationFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var objectResult = context.Result as ObjectResult;

        if (objectResult?.Value is ISerializable serializableObject) // serialize one entity 
        {
            objectResult.Value = serializableObject.ToDto();
        }
        else if (objectResult?.Value is IEnumerable<ISerializable> serializableObjects) // serialize a list of entities
        {
            objectResult.Value = serializableObjects.Select(so => so.ToDto());
        }

        await next();
    }
}
