using LogSummitApi.Application.Core.Services.Summits.Coordinates;
using LogSummitApi.Domain.Core.Utilities;

namespace LogSummitApi.Tests.Unit.Coordinates;

public class CoordinateHelpersTests
{
    [Fact]
    public void AreWithinRange_TrueWhenWithinRange()
    {
        // prepare
        var coord1 = new Coordinate(52.5200, 13.4050, 0); // Berlin
        var coord2 = new Coordinate(52.5200, 13.4060, 0); // close to Berlin
        double range = 1000; // 1 km

        // act & assert
        var result = CoordinateHelpers.IsWithinDistanceFrom(coord1, coord2, range);

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
        var result = CoordinateHelpers.IsWithinDistanceFrom(coord1, coord2, range);

        Assert.False(result);
    }

    [Fact]
    public void AreWithinRange_ThrowsArgumentException_WhenRangeIsZeroOrNegative()
    {
        // prepare
        var coord1 = new Coordinate(52.5200, 13.4050, 0);
        var coord2 = new Coordinate(48.8566, 2.3522, 0);

        // act & assert
        Assert.Throws<ArgumentException>(() => CoordinateHelpers.IsWithinDistanceFrom(coord1, coord2, 0));
        Assert.Throws<ArgumentException>(() => CoordinateHelpers.IsWithinDistanceFrom(coord1, coord2, -10));
    }

    [Fact]
    public void CalculateElevationChange_ShouldReturnCorrectGainAndLoss()
    {
        // prepare
        var coordinates = new List<Coordinate>
        {
            new(37.7749, -122.4194, 10), // San Francisco
            new(34.0522, -118.2437, 20), // Los Angeles
            new(36.1699, -115.1398, 15)  // Las Vegas
        };
        double distanceThreshold = 50000; // 50 km
        double verticalThreshold = 5.0;   // 5 meters

        // act & assert
        var (gain, loss) = CoordinateHelpers.CalculateElevationChange(coordinates, distanceThreshold, verticalThreshold);

        gain.Should().BeApproximately(10.0, 0.1); // gain of 10 meters (20 - 10)
        loss.Should().BeApproximately(5.0, 0.1); // loss of 5 meters (15 - 20)
    }

    [Fact]
    public void CalculateElevationChange_ShouldReturnZeroWhenThresholdsNotMet()
    {
        // prepare
        var coordinates = new List<Coordinate>
        {
            new(37.7749, -122.4194, 10), // San Francisco
            new(34.0522, -118.2437, 10)  // Los Angeles
        };
        var distanceThreshold = 1000000; // 1000 km (very high)
        var verticalThreshold = 50.0;     // 50 meters (very high)

        // act & assert
        var (gain, loss) = CoordinateHelpers.CalculateElevationChange(coordinates, distanceThreshold, verticalThreshold);

        gain.Should().Be(0.0);
        loss.Should().Be(0.0);
    }

    [Fact]
    public void ValidateCoordinatePathAlignments_ShouldReturnTrueWhenMatches()
    {
        // prepare
        var routeCoordinates = new List<Coordinate>
        {
            new(45, 45, 0),
            new(50, 45, 0),
            new(55, 45, 0)
        };

        var attemptCoordinates = new List<Coordinate>
        {
            new(45, 45.000001, 0),   // close to (45, 45)
            new(46, 45.0001, 0),     // between (45, 45) and (50, 45)
            new(48, 45.0005, 0),     // between (45, 45) and (50, 45)
            new(50, 45.00001, 0),    // close to (50, 45)
            new(51, 45.0002, 0),     // between (50, 45) and (55, 45)
            new(53, 45.0005, 0),     // between (50, 45) and (55, 45)
            new(55, 45.00001, 0)     // close to (55, 45)
        };

        // act & assert
        var result = CoordinateHelpers.ValidateCoordinatePathAlignments(routeCoordinates, attemptCoordinates, 10);

        result.Should().BeTrue();
    }

    [Fact]
    public void ValidateCoordinatePathAlignments_ShouldReturnFalseWhenSomeRouteCoordinatesAreNotCovered()
    {
        // Arrange
        var routeCoordinates = new List<Coordinate>
        {
            new(45, 45, 0),
            new(50, 45, 0),
            new(55, 45, 0) 
        };

        var attemptCoordinates = new List<Coordinate>
        {
            new(45, 45.000002, 0),
            new(50.000002, 45.000004, 0) 
            // missing coordinate for (55, 45)
        };

        // act & assert
        bool result = CoordinateHelpers.ValidateCoordinatePathAlignments(routeCoordinates, attemptCoordinates, 10);

        result.Should().BeFalse();
    }
}
