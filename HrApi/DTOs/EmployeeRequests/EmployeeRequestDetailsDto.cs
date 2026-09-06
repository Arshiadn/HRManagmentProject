using HrApi.Enums.Request;

namespace HrApi.DTOs.EmployeeRequests;

public class EmployeeRequestDetailsDto
{
    public long Id { get; set; }

    public int EmployeeId { get; set; }

    public RequestType Type { get; set; }

    public RequestStatus Status { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public int TotalDays { get; set; }

    public string? Destination { get; set; }

    public string? Purpose { get; set; }

    public string? AttachmentPath { get; set; }
}
