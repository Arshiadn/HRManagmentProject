namespace HrApi.Models.Statics;

public static class CriterionCodes
{
    private const string Quality = "QUALITY";
    private const string Collaboration = "COLLABORATION";
    private const string Delivery = "DELIVERY";

    public static readonly string[] All =
    [
        Quality,
        Collaboration,
        Delivery
    ];
}
