using System.ComponentModel.DataAnnotations;

namespace HrApi.DTOs.Employees;

public class CreateEmployeeDto
{
    [Required(ErrorMessage = "نام کامل الزامی است")]
    [MaxLength(200)]
    public string FullName { get; set; }

    [Required(ErrorMessage = "ایمیل الزامی است")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل صحیح نیست")]
    public string Email { get; set; }

    [Range(1, 10, ErrorMessage = "آیدی دپارتمان معتبر نیست")]
    public int DepartmentId { get; set; }

    [Range(0, 100000, ErrorMessage = "حقوق نامعتبر است")]
    public decimal Salary { get; set; }
    public DateTime HireDateFrom { get; set; }
    public DateTime? HireDateTo { get; set; }
}
