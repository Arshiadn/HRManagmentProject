using HrApi.Models;
using HrApi.Interfaces;
using HrApi.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace HrApi.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var result = await _accountService.RegisterAsync(model);
        if (result.IsSuccess)
        {
            return RedirectToAction("Index", "Home");
        }

        foreach(var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty,error);
        }
        return View(model);
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var result = await _accountService.LoginAsync(model);
        if(result.IsSuccess)
        {
            return RedirectToAction("index", "Home");
        }
        ModelState.AddModelError(string.Empty, result.Message);
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _accountService.LogoutAsync();
        return RedirectToAction("index", "Home");
    }
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
