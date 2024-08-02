using LogSummitApi.Domain.Core.Exceptions.HTTP;
using LogSummitApi.Domain.Core.Utilities.Coordinates;

namespace LogSummitApi.Domain.Core.Exceptions;

public class InvalidCoordinateFormatException : BadRequestException
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