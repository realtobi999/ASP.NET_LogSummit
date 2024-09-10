namespace LogSummitApi.Domain.Core.Entities;

public class SuccessMessage
{
    public int Status { get; set; }
    public bool Success { get; set; }
    public object Data { get; set; } = new();
    public string? Instance { get; set; }
}
