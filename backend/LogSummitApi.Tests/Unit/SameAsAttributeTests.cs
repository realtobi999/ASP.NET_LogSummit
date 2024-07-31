using System.ComponentModel.DataAnnotations;
using LogSummitApi.Domain.Core.Attributes.Validation;

namespace LogSummitApi.Tests.Unit;

public class SameAsAttributeTests
{
    [Fact]
    public void SameAsAttribute_PasswordsMatch_ShouldPassValidation()
    {
        // Arrange
        var model = new SampleModel
        {
            Password = "Password123",
            ConfirmPassword = "Password123"
        };

        // Act
        var validationResults = ValidateModel(model);

        // Assert
        Assert.Empty(validationResults); // Expecting no validation errors
    }

    [Fact]
    public void SameAsAttribute_PasswordsDoNotMatch_ShouldFailValidation()
    {
        // Arrange
        var model = new SampleModel
        {
            Password = "Password123",
            ConfirmPassword = "DifferentPassword123"
        };

        // Act
        var validationResults = ValidateModel(model);

        // Assert
        Assert.NotEmpty(validationResults); // Expecting validation errors
        Assert.Contains(validationResults, vr => vr.ErrorMessage!.Contains("ConfirmPassword and Password fields do not match."));
    }

    [Fact]
    public void SameAsAttribute_ComparisonPropertyNotFound_ShouldFailValidation()
    {
        // Arrange
        var model = new SampleModelWithInvalidComparison
        {
            Password = "Password123",
            ConfirmPassword = "Password123"
        };

        // Act
        var validationResults = ValidateModel(model);

        // Assert
        Assert.NotEmpty(validationResults); // Expecting validation errors
        Assert.Contains(validationResults, vr => vr.ErrorMessage!.Contains("Property 'NonExistentProperty' not found."));
    }

    private IList<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, validationContext, validationResults, true);
        return validationResults;
    }

    private class SampleModel
    {
        [Required]
        public string? Password { get; set; }

        [Required]
        [SameAs(nameof(Password))]
        public string? ConfirmPassword { get; set; }
    }

    private class SampleModelWithInvalidComparison
    {
        [Required]
        public string? Password { get; set; }

        [Required]
        [SameAs("NonExistentProperty")]
        public string? ConfirmPassword { get; set; }
    }
}