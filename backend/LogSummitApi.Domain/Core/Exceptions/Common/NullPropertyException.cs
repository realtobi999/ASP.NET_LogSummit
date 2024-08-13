namespace LogSummitApi.Domain.Core.Exceptions.Common;

public class NullPropertyException : NullReferenceException
{
    public NullPropertyException(string entity, string propertyName) : base($"{entity} '{propertyName}' property cannot be null.")
    {
    }
}
