using System.ComponentModel.DataAnnotations;

namespace HrApi.DTOs.Contracts;

public sealed class ActivateContractRequest // submmited => active
{
    public string? Notes { get; set; }
}
