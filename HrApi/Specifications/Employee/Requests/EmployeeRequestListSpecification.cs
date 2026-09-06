using HrApi.DTOs.EmployeeRequests;
using HrApi.Models;
using System.Linq.Expressions;

namespace HrApi.Specifications.Employee.Requests;

public sealed class EmployeeRequestListSpecification
    : BaseSpecification<EmployeeRequest>
{
    public EmployeeRequestListSpecification(
        EmployeeRequestListRequest request)
        : base(EmployeeRequestCriteria.BuildCriteria(request))
    {
        ApplyOrderByDescending(x => x.Id);

        ApplyPaging(
            (request.Page - 1) * request.PageSize,
            request.PageSize);
    }
}
