namespace HrApi.DTOs.Payroll;

public sealed class DepartmentPayrollReportDto
{
    public int DepartmentId { get; init; }
    public string DepartmentName { get; init; } = string.Empty;
    public int EmployeeCount { get; init; }
    public decimal TotalBaseSalary { get; init; }
    public decimal TotalOvertime { get; init; }
    public decimal TotalDeductions { get; init; }
    public decimal TotalPreview { get; init; }
}
