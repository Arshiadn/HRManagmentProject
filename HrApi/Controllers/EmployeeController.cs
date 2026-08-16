using HrApi.DTOs.Employees;
using HrApi.Interfaces;
using HrApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.Controllers;

public class EmployeeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IWebHostEnvironment _environment;

    public EmployeeController(IEmployeeService employeeService,
        IWebHostEnvironment environment)
    {
        _employeeService = employeeService;
        _environment = environment;
    }
    public IActionResult Index()
    {
        var employees = _employeeService.GetAll();
        return View(employees);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([FromForm] CreateEmployeeDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        _employeeService.Create(model);

        TempData["SuccessMessage"] = "کارمند با موفقیت ثبت شد";

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public IActionResult UploadImageProfile(int id)
    {
        var model = new EmployeeProfileImageViewModel
        {
            EmployeeId = id
        };
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadImageProfile(EmployeeProfileImageViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var file = model.ProfileImage;

        var allowedExtensions = new[] { ".jpeg", ".jpg", ".png" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
        {
            ModelState.AddModelError("ProfileImage", "only .jpg, .jpeg, .png is allowed");
            return View(model);
        }

        var maxFileSize = 2 * 1024 * 1024;

        if (file.Length > maxFileSize)
        {
            ModelState.AddModelError("ProfileImage", "Image size must be lower than 2 MegaByte");
            return View(model);
        }

        var uploadFile = Path.Combine(
            _environment.WebRootPath,
            "uploads",
            "profiles");

        if (!Directory.Exists(uploadFile))
        {
            Directory.CreateDirectory(uploadFile);
        }

        var newFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadFile, newFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var relativePath = $"/uploads/profiles/{newFileName}";

        await _employeeService.UploadProfileImageAsync(model.EmployeeId, relativePath);

        TempData["SuccessMessage"] = "پروفایل شما با موفقیت ثبت شد";

        return RedirectToAction(nameof(Index), new { id = model.EmployeeId });
    }
}
