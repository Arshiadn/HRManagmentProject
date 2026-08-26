using HrApi.Models;

namespace HrApi.Specifications.Contracts;

public sealed class FindContractByIdSpecification
    : BaseSpecification<EmployeeContract>
{
    public FindContractByIdSpecification(
        int contractId)
        : base(c => c.Id == contractId)
    {
        AddInclude(c => c.StateHistories);
    }
}
