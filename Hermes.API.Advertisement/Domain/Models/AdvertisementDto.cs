using System;

namespace Hermes.API.Advertisement.Domain.Models
{
    public class AdvertisementDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public long UserId { get; set; }
        public UserDto User { get; set; }

        public Guid CategoryId { get; set; }
        public CategoryDto Category { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}