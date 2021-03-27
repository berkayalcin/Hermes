namespace Hermes.Services.EmailSenderService.Domain.Enums
{
    public enum EmailStatuses
    {
        Deleted = -1,
        Created = 0,
        Locked = 1,
        Sent = 2,
        HasError = 3,
        Delivered = 4,
        Opened = 5,
        MarkedAsSpam = 6,
        Deferred = 7,
        Clicks = 8,
        Blocked = 9,
        Complaint = 10,
        Unsubscribed = 11
    }
}