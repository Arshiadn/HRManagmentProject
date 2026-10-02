using System;

namespace HrApi.DTOs.Positions;

public sealed class PositionDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
