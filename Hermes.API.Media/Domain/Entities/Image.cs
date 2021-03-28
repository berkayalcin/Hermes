using System;

namespace Hermes.API.Media.Domain.Entities
{
    public class Image
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public bool IsDeleted { get; set; }
        public int DisplayOrder { get; set; }
    }
}