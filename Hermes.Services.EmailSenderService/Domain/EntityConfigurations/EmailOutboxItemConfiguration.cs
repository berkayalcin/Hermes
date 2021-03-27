using Hermes.Services.EmailSenderService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Services.EmailSenderService.Domain.EntityConfigurations
{
    public class EmailOutboxItemConfiguration : IEntityTypeConfiguration<EmailOutboxItem>
    {
        public void Configure(EntityTypeBuilder<EmailOutboxItem> builder)
        {
            builder.Property(e => e.Id)
                .HasDefaultValueSql("newid()")
                .ValueGeneratedOnAdd();
            builder.Property(e => e.Body)
                .IsRequired();
            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("getutcdate()")
                .IsRequired();
            builder.Property(e => e.StatusId)
                .IsRequired();
        }
    }
}