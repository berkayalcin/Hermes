using Hermes.Services.EmailSenderService.Domain.Entities;
using Hermes.Services.EmailSenderService.Domain.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hermes.Services.EmailSenderService.Domain.Data
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

        public DbSet<EmailOutboxItem> EmailOutboxItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmailOutboxItemConfiguration());
            base.OnModelCreating(modelBuilder);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetValue<string>("HermesConnectionString");
            if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlServer(connectionString);
        }
    }
}