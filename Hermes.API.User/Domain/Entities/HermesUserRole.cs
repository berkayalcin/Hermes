using Microsoft.AspNetCore.Identity;

namespace Hermes.API.User.Domain.Entities
{
    public class HermesUserRole : IdentityUserRole<long>
    {
        public virtual HermesUser HermesUser { get; set; }
        public virtual HermesRole HermesRole { get; set; }
    }
}