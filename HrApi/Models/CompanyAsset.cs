using HrApi.Enums.Asset;

namespace HrApi.Models;

public sealed class CompanyAsset
{
    public int Id { get; set; }

    public string AssetCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? SerialNumber { get; set; }

    public AssetType Type { get; set; }
    public AssetStatus Status { get; private set; } = AssetStatus.Available;

    public byte[] RowVersion { get; set; } = [];

    public ICollection<AssetAssignment> Assignments { get; set; } = [];

    public void MarkAsAssigned()
    {
        if (Status != AssetStatus.Available)
            throw new InvalidOperationException(
                "only available assets can be assigned.");

        Status = AssetStatus.Assigned;
    }
    public void MarkAsAvailable()
    {
        Status = AssetStatus.Available;
    }
    public void SendToRepair()
    {
        if (Status == AssetStatus.Assigned)
            throw new InvalidOperationException(
                "Assigned asset must be returned first.");

        if (Status == AssetStatus.Lost)
            throw new InvalidOperationException(
                "Lost asset cannot be sent to repair.");

        Status = AssetStatus.UnderRepair;
    }

    public void CompleteRepair()
    {
        if (Status != AssetStatus.UnderRepair)
            throw new InvalidOperationException(
                "Asset is not under repair.");

        Status = AssetStatus.Available;
    }
    public void MarkAsLost()
    {
        if (Status != AssetStatus.Assigned)
            throw new InvalidOperationException(
                "Only assigned assets can be reported lost.");

        Status = AssetStatus.Lost;
    }

    public void MarkAsDamaged()
    {
        if (Status == AssetStatus.Lost)
            throw new InvalidOperationException(
                "Lost asset cannot be marked damaged.");

        Status = AssetStatus.Damaged;
    }
}
