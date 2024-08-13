using LogSummitApi.Domain.Core.Exceptions.Common;
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

    [Fact]
    public void Haversine_CalculatesCorrectDistance()
    {
        // prepare
        var coord1 = new Coordinate(52.5200, 13.4050, 0); // Berlin
        var coord2 = new Coordinate(48.8566, 2.3522, 0);  // Paris
        double expectedDistance = 878_000; // approximate distance in meters

        // act & assert
        var actualDistance = CoordinateMath.Haversine(coord1, coord2);

        Assert.InRange(actualDistance, expectedDistance - 5000, expectedDistance + 5000); // tolerance of 5 km
    }

    [Fact]
    public void AreWithinRange_TrueWhenWithinRange()
    {
        // prepare
        var coord1 = new Coordinate(52.5200, 13.4050, 0); // Berlin
        var coord2 = new Coordinate(52.5200, 13.4060, 0); // close to Berlin
        double range = 1000; // 1 km

        // act & assert
        var result = Coordinate.AreWithinRange(coord1, coord2, range);

        Assert.True(result);
    }

    [Fact]
    public void AreWithinRange_FalseWhenOutsideRange()
    {
        // prepare
        var coord1 = new Coordinate(52.5200, 13.4050, 0); // Berlin
        var coord2 = new Coordinate(48.8566, 2.3522, 0);  // Paris
        double range = 500000; // 500 km

        // act & assert
        var result = Coordinate.AreWithinRange(coord1, coord2, range);

        Assert.False(result);
    }

    [Fact]
    public void AreWithinRange_ThrowsArgumentException_WhenRangeIsZeroOrNegative()
    {
        // prepare
        var coord1 = new Coordinate(52.5200, 13.4050, 0);
        var coord2 = new Coordinate(48.8566, 2.3522, 0);

        // act & assert
        Assert.Throws<ArgumentException>(() => Coordinate.AreWithinRange(coord1, coord2, 0));
        Assert.Throws<ArgumentException>(() => Coordinate.AreWithinRange(coord1, coord2, -10));
    }
}
