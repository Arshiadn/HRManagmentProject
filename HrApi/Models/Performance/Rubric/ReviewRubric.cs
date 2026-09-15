using HrApi.Exceptions;
using HrApi.Models.Performance.Review;

namespace HrApi.Models.Performance.Rubric;

public sealed class ReviewRubric
{
    public Guid Id { get; private set; }
    public int Version { get; private set; }
    public string Name { get; private set; } = null!;
    public List<RubricCriterion> Criteria { get; private set; } = [];

    private ReviewRubric() { }

    private ReviewRubric(
        int version,
        string name)
    {
        if (version <= 0)
        {
            throw new BusinessRuleException(
                "Version باید بزرگ‌تر از صفر باشد.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleException(
                "نام Rubric الزامی است.");
        }
        Id = Guid.NewGuid();
        Version = version;
        Name = name.Trim();
    }
    public static ReviewRubric Create(
        int version,
        string name)
    {
        return new ReviewRubric(
            version,
            name);
    }
    public void AddCriterion(
        string code,
        decimal weight)
    {
        if (Criteria.Any(x =>
            x.Code.Equals(
                code,
                StringComparison.OrdinalIgnoreCase)))
        {
            throw new BusinessRuleException(
                "این Criterion قبلاً اضافه شده است.");
        }

        var criterion = new RubricCriterion(
            Id,
            code,
            weight);

        Criteria.Add(criterion);
    }
    public void EnsureValidWeights()
    {
        var totalWeight = Criteria.Sum(
            x => x.Weight);

        if (totalWeight != 100)
        {
            throw new BusinessRuleException(
                "مجموع Weight معیارها باید 100 باشد.");
        }
    }
}
