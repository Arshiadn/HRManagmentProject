namespace HrApi.DTOs.Payroll;

public sealed class PayrollPreviewDto
{
    public int EmployeeId { get; init; }
    public string EmployeeName { get; init; } = string.Empty;
    public decimal BaseSalary { get; init; }
    public int WorkedMinutes { get; init; }
    public int OvertimeMinutes { get; init; }
    public int DeductionMinutes { get; init; }
    public decimal OvertimeAmount { get; init; }
    public decimal DeductionAmount { get; init; }
    public decimal PreviewAmount { get; init; }
}
