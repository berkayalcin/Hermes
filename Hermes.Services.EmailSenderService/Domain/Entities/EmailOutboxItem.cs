using System;

namespace Hermes.Services.EmailSenderService.Domain.Entities
{
    public class EmailOutboxItem
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public string Error { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? SentAt { get; set; }
        public int StatusId { get; set; }
        public string MessageId { get; set; }
    }
}