using LogSummitApi.Domain.Core;
using LogSummitApi.Domain.Core.Exceptions.Http;

namespace LogSummitApi.Application.Core.Extensions;

public static class IEnumerableExtensions
{
    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int offset, int limit)
    {
        if (offset < 0) throw new BadRequest400Exception("Offset must be zero or greater.");

        if (limit < 0) throw new BadRequest400Exception("Limit must be zero or greater.");

        if (limit > Constants.MaxLimitValue) throw new BadRequest400Exception($"Limit must not exceed the maximum allowed value of {Constants.MaxLimitValue}.");

        if (offset == 0 && limit == 0) return source;

        return limit == 0 ? source.Skip(offset).Take(Constants.MaxLimitValue) : source.Skip(offset).Take(limit);
    }
}
