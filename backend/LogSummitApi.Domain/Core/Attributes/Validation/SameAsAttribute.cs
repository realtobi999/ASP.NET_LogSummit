using System.ComponentModel.DataAnnotations;

namespace LogSummitApi.Domain.Core.Attributes.Validation;

public class SameAsAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public SameAsAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Get the value of the property to compare
        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

        if (property == null)
        {
            return new ValidationResult($"Property '{_comparisonProperty}' not found.");
        }

        var comparisonValue = property.GetValue(validationContext.ObjectInstance);

        // Compare the values
        if (!object.Equals(value, comparisonValue))
        {
            return new ValidationResult($"The {validationContext.DisplayName} and {_comparisonProperty} fields do not match.");
        }

        return ValidationResult.Success;
    }
}
