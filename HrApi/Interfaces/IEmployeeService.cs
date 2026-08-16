using HrApi.DTOs.Employees;
using HrApi.DTOs.Paging;
using HrApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.Interfaces;

public interface IEmployeeService
{
    List<EmployeeListDto> GetAll();
    EmployeeDetailsDto? GetById(int id);
    EmployeeDetailsDto Create(CreateEmployeeDto model);
    void Update(int id, UpdateEmployeeDto model);
    void Delete(int id);
    Task UploadProfileImageAsync(int id, string imagePath);
    Task<PagedResultDto<EmployeeListDto>> Search([FromQuery] EmployeeSearchRequestDto request);
    Task<EmployeePhotoDto> UploadPhotoAsync(int id, [FromForm] EmployeePhotoUploadDto model);
    Task<EmployeePhotoDto> GetPhotoAsync(int id);
    Task<StoredFileResult?> DownloadContractAsync(int id);
    Task DeletePhotoAsync(int id);
}
