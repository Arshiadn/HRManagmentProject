using HrApi.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Security.Claims;

namespace HrApi.Data.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string? UserId =>
        _httpContextAccessor.HttpContext?
            .User?
            .FindFirstValue(ClaimTypes.NameIdentifier);

    public int? EmployeeId
    {
        get
        {
            var claim = _httpContextAccessor
                .HttpContext?
                .User
                .FindFirst("EmployeeId");

            if(claim == null)
                return null;

            return int.TryParse(
                claim.Value,
                out var employeeId
            )
            ? employeeId : null;
        }
    }

    public bool IsInRole(string role)
    {
        return _httpContextAccessor
            .HttpContext?
            .User
            .IsInRole(role) ?? false;
    }
}
