using HrApi.Models;
using HrApi.Enums;
using HrApi.Enums.Contract;

namespace HrApi.Specifications.Employee.Contracts;

public sealed class ActiveContractsSpecification
    : BaseSpecification<EmployeeContract>
{
    public ActiveContractsSpecification()
        : base(e => e.Status == ContractStatus.Active)
    {
        AddInclude(e => e.Employee);

        ApplyOrderBy(e => e.EndDate);
    }
}
