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
        var result = CoordinateHelpers.AreWithinRange(coord1, coord2, range);

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
        var result = CoordinateHelpers.AreWithinRange(coord1, coord2, range);

        Assert.False(result);
    }

    [Fact]
    public void AreWithinRange_ThrowsArgumentException_WhenRangeIsZeroOrNegative()
    {
        // prepare
        var coord1 = new Coordinate(52.5200, 13.4050, 0);
        var coord2 = new Coordinate(48.8566, 2.3522, 0);

        // act & assert
        Assert.Throws<ArgumentException>(() => CoordinateHelpers.AreWithinRange(coord1, coord2, 0));
        Assert.Throws<ArgumentException>(() => CoordinateHelpers.AreWithinRange(coord1, coord2, -10));
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
}
