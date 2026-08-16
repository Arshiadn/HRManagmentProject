using AutoMapper;
using HrApi.Models;
using HrApi.DTOs.Auth;
using HrApi.Interfaces;
using HrApi.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Identity.Client;

namespace HrApi.Data.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _singInManager;
    private readonly IMapper _mapper;

    public AccountService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> singInManager, IMapper mapper)
    {
        _userManager = userManager;
        _singInManager = singInManager;
        _mapper = mapper;
    }
    public async Task<AuthResultDto> RegisterAsync(RegisterViewModel model)
    {
        var resultDto = new AuthResultDto();

        var user = _mapper.Map<ApplicationUser>(model);
                    user.UserName = model.UserName;
                    user.Email = model.Email;
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _singInManager.SignInAsync(user, isPersistent: false);
            resultDto.IsSuccess = true;
            resultDto.Message = "ثبت نام با موفقیت انجام شد";

            return resultDto;
        }

        resultDto.IsSuccess = false;
        resultDto.Message = "ثبت نام موفق نبود";
        resultDto.Errors = result.Errors.Select(x => x.Description).ToList();

        return resultDto;
    }
    public async Task<AuthResultDto> LoginAsync(LoginViewModel model)
    {
        var resultDto = new AuthResultDto();

        var result = await _singInManager.PasswordSignInAsync(
            model.UserName,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            resultDto.IsSuccess = true;
            resultDto.Message = "ورود با موفقیت انجام شد";
            return resultDto;
        }
        resultDto.IsSuccess = false;
        resultDto.Message = "ورود انجام نشد";
        return resultDto;
    }
    public async Task LogoutAsync()
    {
        await _singInManager.SignOutAsync();
    }
}
