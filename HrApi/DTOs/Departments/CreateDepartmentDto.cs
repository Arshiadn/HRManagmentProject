using System.ComponentModel.DataAnnotations;

namespace HrApi.DTOs.Departments;

public class CreateDepartmentDto
{
    [Required(ErrorMessage = "نام الزامی است")]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(100)]
    public string? Description { get; set; }
}
