using HrApi.Enums.Contract;
using HrApi.Models;

namespace HrApi.Specifications.Rules;

public sealed class OverlappingContractSpecification
    : BaseSpecification<EmployeeContract>
{
    public OverlappingContractSpecification(
        int employeeId,
        DateOnly startDate,
        DateOnly endDate,
        int? excludedContractId = null)
        : base(x =>
            x.EmployeeId == employeeId &&
            x.Status != ContractStatus.Cancelled &&
            (!excludedContractId.HasValue ||
             x.Id != excludedContractId.Value) &&
            x.StartDate <= endDate &&
            x.EndDate >= startDate)
    {
    }
}
