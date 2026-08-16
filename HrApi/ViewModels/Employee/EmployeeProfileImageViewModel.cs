using System.ComponentModel.DataAnnotations;

namespace HrApi.ViewModels;

public class EmployeeProfileImageViewModel
{
    [Required]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "عکس پروفایل الزامی است")]
    public IFormFile ProfileImage { get; set; }
}
