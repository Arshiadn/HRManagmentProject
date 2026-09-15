namespace HrApi.DTOs.Reviews.ReviewRubrics;

public sealed class CreateRubricCriterionDto
{
    public string Code { get; init; } = null!;
    public decimal Weight { get; init; }
}
