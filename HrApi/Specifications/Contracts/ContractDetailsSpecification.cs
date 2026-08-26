using HrApi.Models;

namespace HrApi.Specifications.Contracts;

public sealed class ContractDetailsSpecification
    : BaseSpecification<EmployeeContract>
{
    public ContractDetailsSpecification(
        int contractId)
        : base(x => x.Id == contractId)
    {
        AddInclude(x => x.Employee);
        AddInclude(x => x.Employee.Department);
    }
}
