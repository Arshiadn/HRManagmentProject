using HrApi.Enums.Request;

namespace HrApi.DTOs.EmployeeRequests;

public class EmployeeRequestListRequest
{
    public int? EmployeeId { get; set; }

    public RequestType? Type { get; set; }

    public RequestStatus? Status { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}
