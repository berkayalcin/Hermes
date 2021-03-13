using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Hermes.API.User.Domain.Entities
{
    public class HermesRole : IdentityRole<long>
    {
        public ICollection<HermesUserRole> UserRoles { get; set; }
    }
}