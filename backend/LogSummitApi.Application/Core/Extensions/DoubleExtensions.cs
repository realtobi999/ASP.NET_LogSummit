namespace LogSummitApi.Application.Core.Extensions;

public static class DoubleExtensions
{
    public static double ToRadians(this double value)
    {
        return Math.PI * value / 180;
    }
}
