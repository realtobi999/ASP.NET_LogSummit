namespace LogSummitApi.Domain.Core.Entities;

public class ErrorMessage
{
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public string? Title { get; set; }
    public string? Detail { get; set; }
    public string? Type { get; set; }
    public string? Instance { get; set; }
}
