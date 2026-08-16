namespace HrApi.DTOs.Departments;

public class DepartmentDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
