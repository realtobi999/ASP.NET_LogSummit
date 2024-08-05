namespace LogSummitApi.Domain.Core.Attributes;

/// <summary>
/// Attribute used to indicate that a navigation property should be included in querying operations.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class IncludeInQueryingAttribute : Attribute
{

}
