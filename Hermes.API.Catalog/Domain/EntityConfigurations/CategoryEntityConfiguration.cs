using System.Collections.Generic;
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
            builder.HasData(GetInitialCategories());
        }

        private List<Category> GetInitialCategories()
        {
            return new()
            {
                new Category
                {
                    Id = 1,
                    Name = "Elektronik",
                    Description = "Elektronik Eşyalar",
                    Slug = "electronic",
                    IsDeleted = false,
                    ImageUrl = "https://hermes-application-images.s3.eu-central-1.amazonaws.com/laptop.png"
                },
                new Category
                {
                    Id = 2,
                    Name = "Spor & Outdoor",
                    Description = "Spor Outdoor Eşyaları",
                    Slug = "outdoor",
                    IsDeleted = false,
                    ImageUrl = "https://hermes-application-images.s3.eu-central-1.amazonaws.com/camp.png"
                },
                new Category
                {
                    Id = 3,
                    Name = "Araç",
                    Description = "Motorsiklet, Bisiklet ve Arabalar",
                    Slug = "vehicles",
                    IsDeleted = false,
                    ImageUrl = "https://hermes-application-images.s3.eu-central-1.amazonaws.com/vehicle.png"
                },
                new Category
                {
                    Id = 4,
                    Name = "Ev & Bahçe",
                    Description = "Ev & Bahçe Eşyaları",
                    Slug = "garden",
                    IsDeleted = false,
                    ImageUrl = "https://hermes-application-images.s3.eu-central-1.amazonaws.com/garden.png"
                },
                new Category
                {
                    Id = 5,
                    Name = "Moda & Aksesuarlar",
                    Description = "Moda ve Aksesuar Eşyaları",
                    Slug = "fashion-and-accessories",
                    IsDeleted = false,
                    ImageUrl = "https://hermes-application-images.s3.eu-central-1.amazonaws.com/fashion.png"
                },
                new Category
                {
                    Id = 6,
                    Name = "Film & Kitap",
                    Description = "Film & Kitap Eşyaları",
                    Slug = "movies-and-books",
                    IsDeleted = false,
                    ImageUrl = "https://hermes-application-images.s3.eu-central-1.amazonaws.com/book.png"
                },
                new Category
                {
                    Id = 7,
                    Name = "Diğer",
                    Description = "Diğer Eşyalar",
                    Slug = "others",
                    IsDeleted = false,
                    ImageUrl = "https://hermes-application-images.s3.eu-central-1.amazonaws.com/other.png"
                },
            };
        }
    }
}