using HrApi.Enums.Contract;
using System.ComponentModel.DataAnnotations;

namespace HrApi.DTOs.Contracts;

public sealed class CreateContractRequest // create
{
    [Range(1, int.MaxValue)]
    public int EmployeeId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public DateOnly? ProbationEndDate { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal BaseSalary { get; set; }

    public CurrencyCode Currency { get; set; }
}
