using System;
using System.ComponentModel.DataAnnotations;

namespace HrApi.DTOs.Auth;

public sealed class CreateUserDto
{
    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public string Role { get; set; } = string.Empty;
}
