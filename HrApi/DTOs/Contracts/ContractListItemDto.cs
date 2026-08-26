using HrApi.Enums.Contract;

namespace HrApi.DTOs.Contracts;

public sealed class ContractListItemDto // خروجی برای لیست Get All 
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public ContractType ContractType { get; set; }
    public ContractStatus Status { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal BaseSalary { get; set; }
}
