namespace Hermes.API.Advertisement.Domain.Enums
{
    public enum AdvertisementApplicationStatuses
    {
        Created = 1,
        WaitingLenderApproval = 2,
        Approved = 3,
        Rejected = 4,
        WaitingBorrowerApproval = 5,
        Delivered = 6,
        DeliveredBackToLender = 7,
        LenderTookItemBack = 8
    }
}