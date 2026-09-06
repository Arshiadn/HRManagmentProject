using HrApi.DTOs.EmployeeRequests;
using HrApi.Models;

namespace HrApi.Specifications.Employee.Requests;

public sealed class EmployeeRequestCountSpecification
    :BaseSpecification<EmployeeRequest>
{
    public EmployeeRequestCountSpecification(
    EmployeeRequestListRequest request)
    : base(EmployeeRequestCriteria
        .BuildCriteria(request))
    {
    }
}
