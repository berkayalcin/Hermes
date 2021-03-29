using System.Collections.Generic;

namespace Hermes.API.Catalog.Domain.Responses
{
    public class PagedResponse<T>
    {
        public long TotalCount { get; set; }
        public List<T> Items { get; set; }
    }
}