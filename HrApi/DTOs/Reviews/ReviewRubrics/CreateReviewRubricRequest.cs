namespace HrApi.DTOs.Reviews.ReviewRubrics;

public sealed class CreateReviewRubricRequest
{
    public string Name { get; init; } = null!;
    public List<CreateRubricCriterionDto> Criteria { get; init; } = [];
}
