namespace HrApi.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    int? EmployeeId { get; }
    bool IsInRole(string role);
}
