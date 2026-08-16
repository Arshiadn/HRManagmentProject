using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HrApi.Models;
using HrApi.ViewModels;
using System.Reflection.Metadata.Ecma335;

namespace HrApi.Controllers;

public class UsersController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UsersController(UserManager<ApplicationUser> userManager) => _userManager = userManager;
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();

        var modelList = new List<UserInfoViewModel>();

        foreach(var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            modelList.Add(new UserInfoViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                IsActive = user.IsActive,
                Roles = roles
            });
        }
        return View(modelList);
    }
    [HttpGet]
    public async Task<IActionResult> AssignRole(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound();
        return View(user);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignRole(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }
        var result = await _userManager.AddToRoleAsync(user, roleName);

        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = $"The role {roleName} assigned successfuly!";
            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
        return View(user);
    }
    [HttpPost("Users/Deactivate")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        user.IsActive = false;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "User Deactivated";
        }
        else
        {
            TempData["ErrorMessage"] = string.Join(" | ", result.Errors.Select(e => e.Description));
        }

        return RedirectToAction(nameof(Index));
    }
    [HttpPost("Users/Activate")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        user.IsActive = true;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "User Activated";
        }
        else
        {
            TempData["ErrorMessage"] = string.Join(" | ", result.Errors.Select(e => e.Description));
        }

        return RedirectToAction(nameof(Index));
    }
}
