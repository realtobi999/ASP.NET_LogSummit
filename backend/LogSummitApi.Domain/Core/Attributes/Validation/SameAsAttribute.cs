using System.ComponentModel.DataAnnotations;

namespace LogSummitApi.Domain.Core.Attributes.Validation;

/// <summary>
/// A validation attribute that ensures the value of the decorated property matches the value of another specified property.
/// <para>
/// This attribute is used to validate that the value of a property is the same as the value of another property within the same object.
/// It is commonly used for scenarios such as confirming that password and confirm password fields are identical.
/// </para>
/// </summary>
/// <param name="comparisonProperty">
/// The name of the property whose value is used for comparison.
/// </param>
/// <returns>
/// A <c>ValidationResult</c> indicating whether the validation succeeded or failed. If the values do not match, 
/// the result will contain an error message specifying that the properties do not match.
/// </returns>
[AttributeUsage(AttributeTargets.Property)]
public class SameAsAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public SameAsAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // get the value of the property to compare
        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

        if (property == null)
        {
            return new ValidationResult($"Property '{_comparisonProperty}' not found.");
        }

        var comparisonValue = property.GetValue(validationContext.ObjectInstance);

        // compare the values
        if (!object.Equals(value, comparisonValue))
        {
            return new ValidationResult($"The {validationContext.DisplayName} and {_comparisonProperty} fields do not match.");
        }

        return ValidationResult.Success;
    }
}
