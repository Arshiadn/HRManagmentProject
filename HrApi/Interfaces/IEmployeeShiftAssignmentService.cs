using HrApi.DTOs.ShiftAssignment;

namespace HrApi.Interfaces;

public interface IEmployeeShiftAssignmentService
{
    Task<ShiftAssignmentDetailsDto> AssignShiftAsync(
        int employeeId,
        CreateShiftAssignmentDto request,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<ShiftAssignmentDetailsDto>> GetAssignmentsAsync(
        int employeeId,
        CancellationToken cancellationToken);
}
