using HrApi.Enums.Contract;

namespace HrApi.DTOs.Contracts;

public sealed class ContractListRequest // ورودی برای لیست GetAll
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public int? EmployeeId { get; set; }

    public int? DepartmentId { get; set; }

    public ContractStatus? Status { get; set; }

    public ContractType? ContractType { get; set; }

    public decimal? BaseSalary { get; set; }

    public int? ExpiresWithinDays { get; set; }

    public string SortBy { get; set; } = "endDate";

    public string SortDirection { get; set; } = "asc";
}
