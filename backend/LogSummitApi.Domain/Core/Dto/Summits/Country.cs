namespace LogSummitApi.Domain.Core.Dto.Summits;

public record class Country
{
    public CountryNameDto? Name { get; init; }

    public record class CountryNameDto
    {
        public string? Common { get; init; }
        public string? Official { get; init; }
    }
}