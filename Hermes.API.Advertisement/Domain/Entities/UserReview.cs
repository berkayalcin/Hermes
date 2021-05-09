using System;

namespace Hermes.API.Advertisement.Domain.Entities
{
    public class UserReview : DocumentEntityBase<UserReview>
    {
        public Guid Id { get; set; }
        public long ApplicationId { get; set; }
        public long ReviewOwnerId { get; set; }
        public long ReviewedUserId { get; set; }
        public User ReviewOwner { get; set; }
        public User ReviewedUser { get; set; }
        public Advertisement Advertisement { get; set; }
        public string Review { get; set; }
        public int Stars { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}