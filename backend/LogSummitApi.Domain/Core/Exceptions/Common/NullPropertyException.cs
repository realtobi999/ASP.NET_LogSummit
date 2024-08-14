namespace LogSummitApi.Domain.Core.Exceptions.Common;

public class NullPropertyException : NullReferenceException
{
    public NullPropertyException(string entity, string propertyName) : base($"{entity} '{propertyName}' property/field cannot be null.")
    {
    }
}
