using LogSummitApi.Domain.Core.Exceptions;
using LogSummitApi.Domain.Core.Utilities.Coordinates;

namespace LogSummitApi.Tests.Unit;

public class CoordinateTests
{
    [Fact]
    public void Coordinate_ShouldCreateInstance()
    {
        // prepare
        double latitude = 45.0;
        double longitude = 90.0;
        double elevation = 100.0;

        // act & assert
        var coordinate = new Coordinate(latitude, longitude, elevation);

        coordinate.Latitude.Should().Be(latitude);
        coordinate.Longitude.Should().Be(longitude);
        coordinate.Elevation.Should().Be(elevation);
    }

    [Fact]
    public void Coordinate_LongitudeLatitudeElevationValidationWorks()
    {
        // act & assert
        Assert.Throws<InvalidCoordinateFormatException>(() => new Coordinate(99999, 90, 100));
        Assert.Throws<InvalidCoordinateFormatException>(() => new Coordinate(45, 9999, 100));
        Assert.Throws<InvalidCoordinateFormatException>(() => new Coordinate(45, 90, 10000));
    }

    [Fact]
    public void ToString_ReturnCorrectFormat()
    {
        // prepare
        var coordinate = new Coordinate(45, 90, 100);

        // act & assert
        var expected = "45|90|100";

        coordinate.ToString().Should().Be(expected);
    }

    [Fact]
    public void Parse_WorksAndReturnsCorrectInstance()
    {
        // prepare
        var coordinateString = "45|90|100";

        // act & assert
        var expected = new Coordinate(45, 90, 100);

        Coordinate.Parse(coordinateString).Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Parse_ValidationWorks()
    {
        Assert.Throws<InvalidCoordinateFormatException>(() => Coordinate.Parse("hello world!"));
        Assert.Throws<InvalidCoordinateFormatException>(() => Coordinate.Parse("45||0"));
        Assert.Throws<InvalidCoordinateFormatException>(() => Coordinate.Parse("45|"));
        Assert.Throws<InvalidCoordinateFormatException>(() => Coordinate.Parse("45||"));
        Assert.Throws<InvalidCoordinateFormatException>(() => Coordinate.Parse("45|90|hello"));
        Assert.Throws<InvalidCoordinateFormatException>(() => Coordinate.Parse("45|hello|0"));
    }
}
