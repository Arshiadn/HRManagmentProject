namespace HrApi.Models;

public class StoredFileResult
{
    public Byte[] Content { get; set; }
    public string ContentType { get; set; }
    public string DownloadName { get; set; }
}
