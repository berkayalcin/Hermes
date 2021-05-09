using System;
using Hermes.API.Advertisement.Domain.Proxies.Models;

namespace Hermes.API.Advertisement.Domain.Models
{
    public class UserReviewDto
    {
        public Guid Id { get; set; }
        public long ApplicationId { get; set; }
        public long ApplicantId { get; set; }
        public UserDto Lender { get; set; }
        public UserDto Borrower { get; set; }
        public AdvertisementDto Advertisement { get; set; }
        public string Review { get; set; }
        public int Stars { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}