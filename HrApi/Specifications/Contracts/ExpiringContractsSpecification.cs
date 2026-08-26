using HrApi.Models;
using HrApi.Enums.Contract;

namespace HrApi.Specifications.Employee.Contracts;

public class ExpiringContractsSpecification
    : BaseSpecification<EmployeeContract>
{
    public ExpiringContractsSpecification(
        DateOnly today,
        int withinDays)
        : base(e =>
            e.Status == ContractStatus.Active &&
            e.EndDate >= today &&
            e.EndDate <= today.AddDays(withinDays))
    {
        AddInclude(x => x.Employee);

        ApplyOrderBy(x => x.EndDate);
    }
}
