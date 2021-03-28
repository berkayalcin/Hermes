using System.Collections.Generic;

namespace Hermes.API.Media.Domain.Responses
{
    public class ValidationErrorResponse
    {
        public List<string> Errors { get; set; }
    }
}