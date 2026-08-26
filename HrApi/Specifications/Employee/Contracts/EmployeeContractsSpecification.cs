using HrApi.Models;

namespace HrApi.Specifications.Employee.Contracts;

public class EmployeeContractsSpecification
    : BaseSpecification<EmployeeContract>
{
    public EmployeeContractsSpecification(
        int employeeId) 
        : base(e => e.EmployeeId == employeeId)
    {
        ApplyOrderByDescending(e => e.StartDate);
    }
}
