using LogSummitApi.Application.Core.Validators;
using LogSummitApi.Domain.Core.Dto.Summits;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Tests.Helpers.Extensions;
using Moq;

namespace LogSummitApi.Tests.Unit.Validators;

public class SummitValidatorTests
{
    private readonly Mock<IRepositoryManager> _mock;
    private readonly SummitValidator _validator;

    public SummitValidatorTests()
    {
        _mock = new Mock<IRepositoryManager>();
        _validator = new SummitValidator(_mock.Object);
    }

    [Fact]
    public async void IsValidAsync_ReturnsFalseWhenUserDoesntExist()
    {
        // prepare
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);

        _mock.Setup(r => r.Summit.IndexAsync()).ReturnsAsync([]);
        _mock.Setup(r => r.HttpCountry.IndexAsync()).ReturnsAsync([]);
        _mock.Setup(r => r.Users.GetAsync(summit.UserId)).ReturnsAsync((User?)null); // return null => user was not found

        // act & assert
        var (isValid, exception) = await _validator.IsValidAsync(summit);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested {nameof(User)} with the key '{summit.UserId}' was not found in the system.");
    }

    [Fact]
    public async void IsValidAsync_ReturnsFalseWhenTwoSummitsAreCloseTogether()
    {
        // prepare
        var user = new User().WithFakeData();

        var summit1 = new Summit().WithFakeData(user); // summits are both public
        var summit2 = new Summit().WithFakeData(user);

        summit2.Coordinate = summit1.Coordinate; // set the second summit coordinate to the first => we break the summit proximity radius

        _mock.Setup(r => r.Summit.IndexAsync()).ReturnsAsync([summit1, summit2]);
        _mock.Setup(r => r.HttpCountry.IndexAsync()).ReturnsAsync([]);
        _mock.Setup(r => r.Users.GetAsync(summit1.UserId)).ReturnsAsync(user);

        // act & assert
        var (isValid, exception) = await _validator.IsValidAsync(summit1);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<BadRequest400Exception>();
        exception!.Message.Should().Be($"A summit already exists within a {Summit.SUMMIT_PROXIMITY_RADIUS}-meter radius.");
    }

    [Fact]
    public async Task IsValidAsync_ReturnsFalseWhenCountryIsNotValidAsync()
    {
        // prepare
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);

        summit.Country = "NOT_A_VALID_COUNTRY";

        _mock.Setup(r => r.Summit.IndexAsync()).ReturnsAsync([summit]);
        _mock.Setup(r => r.HttpCountry.IndexAsync()).ReturnsAsync([new Country() { Name = new Country.CountryNameDto { Common = "Czechia", Official = "Czech Republic" } }]);
        _mock.Setup(r => r.Users.GetAsync(summit.UserId)).ReturnsAsync(user);

        // act & assert

        var (isValid, exception) = await _validator.IsValidAsync(summit);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<BadRequest400Exception>();
        exception!.Message.Should().Be($"A '{summit.Country}' is not a valid country. List of all available countries: GET /v1/api/summit/valid-countries");
    }
}
