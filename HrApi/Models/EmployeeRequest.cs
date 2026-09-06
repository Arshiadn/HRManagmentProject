using HrApi.Enums.Request;

namespace HrApi.Models;

public sealed class EmployeeRequest
{
    public long Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public RequestType Type { get; set; }
    public RequestStatus Status { get; private set; }
        = RequestStatus.Draft;

    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public int TotalDays { get; private set; }
    public string? Destination { get; set; }
    public string? Purpose { get; set; }
    public string? AttachmentPath { get; set; }

    public Byte[] RowVersion { get; set; } = [];

    public void Submit()
    {
        if(Status != RequestStatus.Draft)
        {
            throw new InvalidOperationException(
                "Only draft requests can be submitted");
        }

        Status = RequestStatus.Submitted;
    }
    public void Approve()
    {
        if (Status != RequestStatus.Submitted)
        {
            throw new InvalidOperationException(
                "Only submitted requests can be approved.");
        }

        Status = RequestStatus.Approved;
    }
    public void Reject()
    {
        if (Status != RequestStatus.Submitted)
        {
            throw new InvalidOperationException(
                "Only submitted requests can be rejected.");
        }

        Status = RequestStatus.Rejected;
    }
    public void Cancel()
    {
        if (Status == RequestStatus.Approved)
        {
            throw new InvalidOperationException(
                "Approved requests cannot be cancelled.");
        }

        Status = RequestStatus.Cancelled;
    }
}
