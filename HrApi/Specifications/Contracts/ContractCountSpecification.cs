using HrApi.DTOs.Contracts;
using HrApi.Models;
using HrApi.Specifications.Common;

namespace HrApi.Specifications.Contracts;

public sealed class ContractCountSpecification
    : BaseSpecification<EmployeeContract>
{
    public ContractCountSpecification(
        ContractListRequest request,
        DateOnly today)
        : base(ContractCriteria.BuildCriteria(request, today))
    {
    }
}
