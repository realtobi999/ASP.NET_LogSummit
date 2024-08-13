namespace LogSummitApi.Domain.Core.Interfaces.Common;

public interface ISerializable<out Dto> : ISerializable
{
    new Dto? ToDto();
}

public interface ISerializable
{
    object? ToDto()
    {
        return ((ISerializable<object>)this).ToDto();
    }
}
