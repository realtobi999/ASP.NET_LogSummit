namespace LogSummitApi.Domain.Core.Dto.Summits;

public record class Country
{
    public CountryNameDto? Name { get; set; }

    public record class CountryNameDto
    {
        public string? Common { get; set; }
        public string? Official { get; set; }
    }
}