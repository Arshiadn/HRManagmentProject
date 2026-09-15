using HrApi.Models.Performance.Review;
using HrApi.Models.Performance.Rubric;

namespace HrApi.Calculators;

public static class PerformanceScoreCalculator
{
    public static decimal CalculateOverall(
        IReadOnlyCollection<ReviewScore> scores,
        IReadOnlyCollection<RubricCriterion> criteria)
    {
        var byCode = scores.ToDictionary(x => x.CriterionCode);
        return criteria.Sum(
            c => byCode[c.Code].Score * (c.Weight / 100m));
    }
}
