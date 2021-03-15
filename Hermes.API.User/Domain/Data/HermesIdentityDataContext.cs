using System.Diagnostics.CodeAnalysis;
using Hermes.API.User.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hermes.API.User.Domain.Data
{
    [ExcludeFromCodeCoverage]
    public class HermesIdentityDbContext : IdentityDbContext<HermesUser, HermesRole, long, IdentityUserClaim<long>,
        HermesUserRole, IdentityUserLogin<long>,
        IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        private readonly IConfiguration _configuration;

        public HermesIdentityDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public HermesIdentityDbContext(DbContextOptions<HermesIdentityDbContext> options,
            IConfiguration configuration) :
            base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<HermesUserRole>(userRole =>
            {
                userRole.HasOne(ur => ur.HermesRole)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.HermesUser)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetValue<string>("HermesIdentityConnectionString");
            if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlServer(connectionString);
        }
    }
}