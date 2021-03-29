using Hermes.API.Catalog.Domain.Entities;
using Hermes.API.Catalog.Domain.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hermes.API.Catalog.Domain.Data
{
    public class HermesDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

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

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetValue<string>("HermesConnectionString");
            if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlServer(connectionString);
        }
    }
}