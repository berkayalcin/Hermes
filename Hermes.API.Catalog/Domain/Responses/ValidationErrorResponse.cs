using System.Collections.Generic;

namespace Hermes.API.Catalog.Domain.Responses
{
    public class ValidationErrorResponse
    {
        public List<string> Errors { get; set; }
    }
}