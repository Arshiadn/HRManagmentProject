using HrApi.DTOs.Contracts;
using HrApi.Enums.Contract;
using HrApi.Models;
using System.Linq.Expressions;

namespace HrApi.Specifications.Common;

public static class ContractCriteria
{
    public static Expression<Func<EmployeeContract, bool>> BuildCriteria(
        ContractListRequest request,
        DateOnly today)
    {
        return x =>
        (!request.EmployeeId.HasValue ||
            x.EmployeeId == request.EmployeeId.Value) &&

        (!request.DepartmentId.HasValue ||
            x.Employee.DepartmentId == request.DepartmentId.Value) &&

        (!request.Status.HasValue ||
             x.Status == request.Status.Value) &&

            (!request.ContractType.HasValue ||
                 x.ContractType == request.ContractType.Value) &&

             (!request.BaseSalary.HasValue ||
                 x.BaseSalary <= request.BaseSalary.Value) &&

        (!request.ExpiresWithinDays.HasValue ||
             (x.Status == ContractStatus.Active &&
                  x.EndDate >= today &&
                  x.EndDate <=
                  today.AddDays(request.ExpiresWithinDays.Value)));
    }
}
