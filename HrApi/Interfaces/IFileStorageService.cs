namespace HrApi.Interfaces;
using HrApi.Models;

public interface IFileStorageService
{
    Task<string> SavePublicImageAsync(
       IFormFile file,
       string folderName);

    Task DeletePublicFileAsync(string relativePath);

    Task<StoredFileResult?> GetPrivateFileAsync(
        string relativePath
    );
}
