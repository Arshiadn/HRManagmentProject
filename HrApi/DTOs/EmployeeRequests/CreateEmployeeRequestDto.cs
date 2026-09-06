using HrApi.Enums.Request;

namespace HrApi.DTOs.EmployeeRequests;

public sealed class CreateEmployeeRequestDto
{
    public int EmployeeId { get; set; }

    public RequestType Type { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public string? Destination { get; set; } = string.Empty;

    public string? Purpose { get; set; } = string.Empty;

    public string? AttachmentPath { get; set; } = string.Empty;
}
