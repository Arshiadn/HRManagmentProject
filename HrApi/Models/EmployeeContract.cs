using HrApi.Enums.Contract;

namespace HrApi.Models;

public sealed class EmployeeContract
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public ContractType ContractType { get; set; }

    public ContractStatus Status { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public DateOnly? ProbationEndDate { get; set; }

    public decimal BaseSalary { get; set; }

    public CurrencyCode Currency { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public string? Notes { get; set; }

    public double? ScoreRate { get; set; }

    public byte[] RowVersion { get; set; } = [];

    public ICollection<ContractStateHistory> StateHistories { get; set; } = new List<ContractStateHistory>();
}