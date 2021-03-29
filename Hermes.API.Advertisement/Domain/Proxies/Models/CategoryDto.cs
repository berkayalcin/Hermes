namespace Hermes.API.Advertisement.Domain.Proxies.Models
{
    public class CategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public string ImageUrl { get; set; }
    }
}