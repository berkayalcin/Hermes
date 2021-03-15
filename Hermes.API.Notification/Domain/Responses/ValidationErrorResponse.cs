using System.Collections.Generic;

namespace Hermes.API.Notification.Domain.Responses
{
    public class ValidationErrorResponse
    {
        public List<string> Errors { get; set; }
    }
}