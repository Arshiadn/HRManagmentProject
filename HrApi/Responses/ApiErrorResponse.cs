namespace HrApi.Responses;

public class ApiErrorResponse
{
    public bool Success { get; set; } = false;
    public string Message { get; set; }
    public List<string> Errors { get; set; } = new();
}
