using HrApi.DTOs.EmployeeRequests;
using HrApi.Models;
using System.Linq.Expressions;

namespace HrApi.Specifications.Employee.Requests;

public static class EmployeeRequestCriteria
{
    public static Expression<Func<EmployeeRequest, bool>>
        BuildCriteria(
            EmployeeRequestListRequest request)
    {
        return x =>
            (!request.EmployeeId.HasValue ||
             x.EmployeeId == request.EmployeeId.Value)

            &&

            (!request.Type.HasValue ||
             x.Type == request.Type.Value)

            &&

            (!request.Status.HasValue ||
             x.Status == request.Status.Value)

            &&

            (!request.FromDate.HasValue ||
             x.FromDate >= request.FromDate.Value)

            &&

            (!request.ToDate.HasValue ||
             x.ToDate <= request.ToDate.Value);
    }
}
