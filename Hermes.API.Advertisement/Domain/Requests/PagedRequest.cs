namespace Hermes.API.Advertisement.Domain.Requests
{
    public abstract class PagedRequest
    {
        protected PagedRequest()
        {
            OrderBy = "Id";
            OrderByDesc = true;
        }

        public int PageSize { get; set; }
        public int SkipCount { get; set; }
        public string OrderBy { get; set; }
        public bool OrderByDesc { get; set; }
        public string Query { get; set; }
    }
}