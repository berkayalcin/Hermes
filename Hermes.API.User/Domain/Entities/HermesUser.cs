using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Hermes.API.User.Domain.Entities
{
    public class HermesUser : IdentityUser<long>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool IsAcceptedEula { get; set; } = true;
        public bool IsDeleted { get; set; }
        public ICollection<HermesUserRole> UserRoles { get; set; }
    }
}