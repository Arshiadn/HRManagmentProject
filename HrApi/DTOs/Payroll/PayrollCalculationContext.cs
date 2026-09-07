namespace HrApi.DTOs.Payroll;

public sealed class PayrollCalculationContext
{
    public decimal BaseSalary { get; init; }
    public int OvertimeMinutes { get; init; }
    public int DeductionMinutes { get; init; }
    public decimal OvertimeAmount { get; set; }
    public decimal DeductionAmount { get; set; }
    public decimal Total { get; set; }
}
