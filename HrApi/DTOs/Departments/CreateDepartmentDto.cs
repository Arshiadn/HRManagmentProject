using System.ComponentModel.DataAnnotations;

namespace HrApi.DTOs.Departments;

public class CreateDepartmentDto
{
    [Required(ErrorMessage = "نام الزامی است")]
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
}
