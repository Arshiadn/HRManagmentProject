using HrApi.Enums.Contract;
using HrApi.Models;

namespace HrApi.Specifications.Employee.Contracts;

public class ActiveEmployeeContractSpecification
    : BaseSpecification<EmployeeContract>
{
    public ActiveEmployeeContractSpecification(
    int employeeId)
    : base(x =>
        x.EmployeeId == employeeId &&
        x.Status == ContractStatus.Active)
    {
        ApplyOrderByDescending(x => x.StartDate);
    }
}
