using System.ComponentModel.DataAnnotations;

namespace HrApi.ViewModels;
public class LoginViewModel
{
    [Required(ErrorMessage = "نام کاربری الزامی است")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "رمز عبور الزامی است")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}
