using HrApi.DTOs.Auth;
using HrApi.ViewModels;

namespace HrApi.Interfaces;

public interface IAccountService
{
    Task<AuthResultDto> RegisterAsync(RegisterViewModel model);
    Task<AuthResultDto> LoginAsync(LoginViewModel model);
    Task LogoutAsync();
}
