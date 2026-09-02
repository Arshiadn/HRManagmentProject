using HrApi.DTOs.Attendance;
using HrApi.DTOs.Paging;

namespace HrApi.Interfaces;

public interface IAttendanceService
{
    Task<AttendanceDailyDto> CheckIn(
        int employeeId,
        CancellationToken cancellationToken);

    Task<AttendanceDailyDto> CheckOut(
        int employeeId,
        CancellationToken cancellationToken);

    Task<AttendanceDailyDto?> GetDaily(
        int employeeId,
        CancellationToken cancellationToken);

    Task<PagedResultDto<AttendanceDailyDto>> GetEmployeeAttendance(
        int employeeId,
        AttendanceListRequestDto request,
        CancellationToken cancellationToken);
}
