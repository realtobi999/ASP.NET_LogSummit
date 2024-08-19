﻿using System.ComponentModel.DataAnnotations;
using LogSummitApi.Domain.Core.Utilities;

namespace LogSummitApi.Domain.Core.Dto.Summits;

public record class UpdateSummitDto
{
    [Required, MinLength(3), MaxLength(155)]
    public string? Name { get; set; }

    [Required, MinLength(15), MaxLength(1555)]
    public string? Description { get; set; }

    [Required, MaxLength(155)]
    public string? Country { get; set; }

    [Required]
    public bool? IsPublic { get; set; }

    [Required]
    public Coordinate? Coordinate { get; set; }
}