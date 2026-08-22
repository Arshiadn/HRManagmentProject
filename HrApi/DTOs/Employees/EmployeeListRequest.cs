using System.ComponentModel.DataAnnotations;

namespace HrApi.DTOs.Employees;

public sealed class EmployeeListRequest
{
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;
    [Range(1, 100)]
    public int PageSize { get; set; } = 20;
    [MaxLength(100)]
    public string? Search {  get; set; }
    public int? DepartmentId { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? HireDateFrom { get; set; }
    public DateTime? HireDateTo { get; set; }
    public string SortBy { get; set; } = "FullName";
    [RegularExpression("asc|desc")]
    public string SortDirection { get; set; } = "asc";
}
