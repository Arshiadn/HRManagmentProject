namespace HrApi.Enums.Contract;

public enum ContractStatus
{
    WaitingForSignature = 1,
    Signed = 2,
    Active = 3,
    Expired = 4,
    Cancelled = 5,
    Renewed = 6,
    Completed = 7
}
