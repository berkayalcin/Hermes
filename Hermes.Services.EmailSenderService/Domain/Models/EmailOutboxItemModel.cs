using System.Collections.Generic;

namespace Hermes.Services.EmailSenderService.Domain.Models
{
    public class EmailOutboxItemModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> To { get; set; }
        public List<string> Bcc { get; set; }
        public List<string> Cc { get; set; }
    }
}