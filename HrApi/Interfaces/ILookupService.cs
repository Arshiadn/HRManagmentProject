using HrApi.DTOs.Lookup;

namespace HrApi.Interfaces;

public interface ILookupService 
{
    Task<IReadOnlyList<LookupItemDto>> GetActiveDepartmentsAsync(
        CancellationToken cancellationToken);
}
