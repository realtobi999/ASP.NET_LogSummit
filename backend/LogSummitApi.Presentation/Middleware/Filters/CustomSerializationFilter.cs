using LogSummitApi.Domain.Core.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LogSummitApi.Presentation.Middleware.Filters;

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
