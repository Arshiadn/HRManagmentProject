using HrApi.Interfaces;
using HrApi.Models;

namespace HrApi.Data.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;
    private static readonly string[] AllowedExtensions =
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };
    private static readonly string[] AllowedContentTypes =
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };
    private const long maxFileSize = 1024 * 1024 * 2;
    public LocalFileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    private static void ValidateImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new InvalidOperationException("فایل تصویر ارسال نشده است");
        }
        if (file.Length > maxFileSize)
        {
            throw new InvalidOperationException(
                "حجم فایل بیشتر از حد مجاز است"
            );
        }
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException(
                "پسوند فایل مجاز نیست"
            );
        }
        if (!AllowedContentTypes.Contains(file.ContentType))
        {
            throw new InvalidOperationException(
                "نوع فایل مجاز نیست"
            );
        }
    }
    public async Task<string> SavePublicImageAsync(IFormFile file, string folderName)
    {
        ValidateImage(file);

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        var fileName = $"{Guid.NewGuid():N}{extension}";

        var folderPath = Path.Combine(
            _environment.WebRootPath,
            "uploads",
            folderName);
        Directory.CreateDirectory(folderPath);

        var physicalPath = Path.Combine(folderPath, fileName);

        await using var stream = new FileStream(physicalPath, FileMode.Create);

        await file.CopyToAsync(stream);

        return $"/uploads/{folderName}/{fileName}";
    }
    public Task DeletePublicFileAsync(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return Task.CompletedTask;
        }
        var safePath = relativePath.TrimStart('/');

        var physicalPath = Path.Combine(
            _environment.WebRootPath,
            safePath.Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
        }
        return Task.CompletedTask;
    }
    public async Task<StoredFileResult?> GetPrivateFileAsync(string relativePath)
    {
        var privateRoot = Path.Combine(
            _environment.WebRootPath,
            "private-files");

        var physicalPath = Path.Combine(privateRoot, relativePath);

        if (!File.Exists(physicalPath))
        {
            return null;
        }
        var content = await File.ReadAllBytesAsync(physicalPath);

        return new StoredFileResult
        {
            Content = content,
            ContentType = "application/octet-stream",
            DownloadName = Path.GetFileName(physicalPath)
        };
    }
}
