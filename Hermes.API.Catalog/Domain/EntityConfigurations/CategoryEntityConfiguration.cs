using Hermes.API.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.API.Catalog.Domain.EntityConfigurations
{
    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(p => p.Id)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(p => p.Slug)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(p => p.Description)
                .HasMaxLength(2000)
                .IsRequired();
            builder.Property(p => p.ImageUrl)
                .HasMaxLength(2000)
                .IsRequired();
            builder.Property(p => p.IsDeleted)
                .HasDefaultValueSql("0")
                .IsRequired();
        }
    }
}