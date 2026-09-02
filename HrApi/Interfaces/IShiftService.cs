using HrApi.DTOs.Shifts;
using HrApi.Responses;

namespace HrApi.Interfaces;

public interface IShiftService
{
    // Create => 200
    Task<ShiftDetailsDto> Create(
        CreateShiftDto request,
        CancellationToken cancellationToken);
    // Update => 204
    Task Update(
        int id,
        UpdateShiftDto request,
        CancellationToken cancellationToken);
}
