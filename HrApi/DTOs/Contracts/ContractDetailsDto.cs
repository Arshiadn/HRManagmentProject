using HrApi.Enums.Contract;
using HrApi.Models;

namespace HrApi.DTOs.Contracts;

public sealed class ContractDetailsDto // Get By Id
{
    public int Id { get; init; }

    public int EmployeeId { get; init; }

    public ContractType ContractType { get; init; }

    public ContractStatus Status { get; init; }

    public DateOnly StartDate { get; init; }

    public DateOnly EndDate { get; init; }

    public decimal BaseSalary { get; init; }

    public CurrencyCode Currency { get; init; }

    public IReadOnlyList<ContractStateHistory> StateHistories { get; init; }
        = Array.Empty<ContractStateHistory>();
}
