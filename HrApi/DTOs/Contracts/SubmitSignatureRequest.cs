using HrApi.Enums.Contract;

namespace HrApi.DTOs.Contracts;

public sealed class SubmitSignatureRequest // submit for signature => submmited
{
    public ContractType ContractType { get; set; }
    public string? Notes { get; set; }
}
