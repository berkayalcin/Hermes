using System.Collections.Generic;

namespace Hermes.API.User.Domain.Responses
{
    public class ValidationErrorResponse
    {
        public List<string> Errors { get; set; }
    }
}