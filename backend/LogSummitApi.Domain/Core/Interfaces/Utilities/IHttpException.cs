namespace LogSummitApi.Domain.Core.Interfaces.Utilities;

public interface IHttpException
{
    public int StatusCode { get; }
    public string Title { get; }
    public string Message { get; }
}
