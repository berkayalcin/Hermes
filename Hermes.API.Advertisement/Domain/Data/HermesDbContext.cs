using Hermes.API.Advertisement.Domain.Entities;
using Hermes.API.Advertisement.Domain.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hermes.API.Advertisement.Domain.Data
{
    public class HermesDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<AdvertisementApplication> AdvertisementApplications { get; set; }

        public HermesDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public HermesDbContext(DbContextOptions<HermesDbContext> options,
            IConfiguration configuration) :
            base(options)
        {
            _configuration = configuration;
        }

        public HermesDbContext()
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdvertisementApplicationEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetValue<string>("HermesConnectionString");
            if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlServer(connectionString);
        }
    }
}