using HrApi.DTOs.Payroll;

namespace HrApi.Calculators;

public static class PayrollCalculationPipeline
{
    private const int standardMonthlyMinutes = 176 * 60;

    public static void ApplyBaseSalary(
        PayrollCalculationContext context)
    {
        context.Total = context.BaseSalary;
    }

    public static void ApplyOvertime(
        PayrollCalculationContext context)
    {
        var minuteRate = 
            context.BaseSalary / standardMonthlyMinutes;

        context.OvertimeAmount =
            context.OvertimeMinutes * minuteRate * 1.4m;

        context.Total += context.OvertimeAmount;
    }
    public static void ApplyDeduction(
        PayrollCalculationContext context)
    {
        var minuteRate =
            context.BaseSalary / standardMonthlyMinutes;

        context.DeductionAmount =
            context.DeductionMinutes * minuteRate;

        context.Total -= context.DeductionAmount;
    }
}
