namespace LogSummitApi.Domain.Core.Dto.Summit;

public record class CountryDto
{
    public NameDto? Name { get; set; }

    public record class NameDto
    {
        public string? Common { get; set; }
        public string? Official { get; set; }
    }
}