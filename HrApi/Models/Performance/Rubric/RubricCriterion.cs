using HrApi.Exceptions;

namespace HrApi.Models.Performance.Rubric;

public sealed class RubricCriterion
{
    public Guid ReviewRubricId { get; private set; }
    public ReviewRubric ReviewRubric { get; private set; } = null!;

    public string Code { get; private set; } = null!;
    public decimal Weight { get; private set; }

    private RubricCriterion()
    {
    }
    public RubricCriterion(
        Guid reviewRubricId,
        string code,
        decimal weight)
    {
        EnsureValidCode(code);
        EnsureValidWeight(weight);

        ReviewRubricId = reviewRubricId;
        Code = code.Trim().ToUpperInvariant();
        Weight = weight;
    }
    private static void EnsureValidCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new BusinessRuleException(
                "Criterion code الزامی است.");
        }
    }
    private static void EnsureValidWeight(decimal weight)
    {
        if (weight <= 0 || weight > 100)
        {
            throw new BusinessRuleException(
                "Weight باید بین 0 و 100 باشد.");
        }
    }
}
