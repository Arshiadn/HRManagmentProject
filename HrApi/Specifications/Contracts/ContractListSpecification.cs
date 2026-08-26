using HrApi.DTOs.Contracts;
using HrApi.Enums.Contract;
using HrApi.Models;
using HrApi.Specifications.Common;
using System.Linq.Expressions;

namespace HrApi.Specifications.Employee.Contracts;

public sealed class ContractListSpecification
    : BaseSpecification<EmployeeContract>
{
    public ContractListSpecification(
        ContractListRequest request,
        DateOnly today)
        : base(ContractCriteria.BuildCriteria(request, today))
    {
        AddInclude(x => x.Employee);
        AddInclude(x => x.Employee.Department);

        ApplySorting(request);

        ApplyPaging(
            (request.Page - 1) * request.PageSize,
            request.PageSize);
    }
    private void ApplySorting(
        ContractListRequest request)
    {
        var descending = request.SortDirection.Equals(
            "desc",
            StringComparison.OrdinalIgnoreCase);

        switch (request.SortBy.ToLowerInvariant())
        {
            case "startdate":
                if (descending)
                    ApplyOrderByDescending(x => x.StartDate);
                else
                    ApplyOrderBy(x => x.StartDate);
                break;

            case "salary":
                if (descending)
                    ApplyOrderByDescending(x => x.BaseSalary);
                else
                    ApplyOrderBy(x => x.BaseSalary);
                break;

            default:
                if (descending)
                    ApplyOrderByDescending(x => x.EndDate);
                else
                    ApplyOrderBy(x => x.EndDate);
                break;
        }
    }
}
