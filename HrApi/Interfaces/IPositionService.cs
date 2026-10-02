using System;
using HrApi.DTOs.Positions;

namespace HrApi.Interfaces;

public interface IPositionService
{
        Task<PositionDetailsDto> CreateAsync(
        CreatePositionDto dto,
        CancellationToken cancellationToken);

        Task<List<PositionDetailsDto>> GetAllAsync(
        CancellationToken cancellationToken);

        Task<PositionDetailsDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

        Task<PositionDetailsDto?> UpdateAsync(
        int id,
        UpdatePositionDto dto,
        CancellationToken cancellationToken);

        Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken);
}
