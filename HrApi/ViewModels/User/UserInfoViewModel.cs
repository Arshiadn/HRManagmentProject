namespace HrApi.ViewModels;

public class UserInfoViewModel
{
    public string Id { get; set; } = default!;
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
}
