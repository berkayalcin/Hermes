using System.Collections.Generic;

namespace Hermes.API.Advertisement.Domain.Responses
{
    public class ValidationErrorResponse
    {
        public List<string> Errors { get; set; }
    }
}