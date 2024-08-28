using GeoCoordinates.Core;
using LogSummitApi.Domain.Core.Exceptions.Http;

namespace LogSummitApi.Domain.Core.Exceptions.Common;

public class InvalidCoordinateFormatException : BadRequest400Exception
{
    public InvalidCoordinateFormatException(string message)
       : base($"Invalid coordinate format: {message}")
    {
    }
    public InvalidCoordinateFormatException(Coordinate coordinate, string message)
        : base($"Invalid coordinate format: {message} (Latitude: {coordinate.Latitude}, Longitude: {coordinate.Longitude}, Elevation: {coordinate.Elevation})")
    {
    }
}