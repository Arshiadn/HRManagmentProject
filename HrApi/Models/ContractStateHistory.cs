using HrApi.Enums;
using HrApi.Enums.Contract;

namespace HrApi.Models;

public sealed class ContractStateHistory
{
    public int Id { get; set; }

    public int ContractId { get; set; }

    public int EmployeeId { get; set; }

    public ContractStatus FromState{ get; set; }

    public ContractStatus ToState { get; set; }

    public string? Reason { get; set; }

    public DateTime ChangedAtUtc { get; set; }

    public string ChangedByUserId { get; set; } = string.Empty;
}
