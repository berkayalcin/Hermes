// <auto-generated />

using Hermes.API.Catalog.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hermes.API.Catalog.Migrations
{
    [DbContext(typeof(HermesDbContext))]
    partial class HermesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Hermes.API.Catalog.Domain.Entities.Category", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint")
                    .HasAnnotation("SqlServer:IdentityIncrement", 1)
                    .HasAnnotation("SqlServer:IdentitySeed", 1)
                    .HasAnnotation("SqlServer:ValueGenerationStrategy",
                        SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("Description")
                    .IsRequired()
                    .HasMaxLength(2000)
                    .HasColumnType("nvarchar(2000)");

                b.Property<string>("ImageUrl")
                    .IsRequired()
                    .HasMaxLength(2000)
                    .HasColumnType("nvarchar(2000)");

                b.Property<bool>("IsDeleted")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValueSql("0");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)");

                b.Property<string>("Slug")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)");

                b.HasKey("Id");

                b.ToTable("Categories");

                b.HasData(
                    new
                    {
                        Id = 1L,
                        Description = "Elektronik Eşyalar",
                        ImageUrl = "https://hermes-application-images.s3.eu-central-1.amazonaws.com/laptop.png",
                        IsDeleted = false,
                        Name = "Elektronik",
                        Slug = "electronic"
                    },
                    new
                    {
                        Id = 2L,
                        Description = "Spor Outdoor Eşyaları",
                        ImageUrl = "https://hermes-application-images.s3.eu-central-1.amazonaws.com/camp.png",
                        IsDeleted = false,
                        Name = "Spor & Outdoor",
                        Slug = "outdoor"
                    },
                    new
                    {
                        Id = 3L,
                        Description = "Motorsiklet, Bisiklet ve Arabalar",
                        ImageUrl = "https://hermes-application-images.s3.eu-central-1.amazonaws.com/vehicle.png",
                        IsDeleted = false,
                        Name = "Araç",
                        Slug = "vehicles"
                    },
                    new
                    {
                        Id = 4L,
                        Description = "Ev & Bahçe Eşyaları",
                        ImageUrl = "https://hermes-application-images.s3.eu-central-1.amazonaws.com/garden.png",
                        IsDeleted = false,
                        Name = "Ev & Bahçe",
                        Slug = "garden"
                    },
                    new
                    {
                        Id = 5L,
                        Description = "Moda ve Aksesuar Eşyaları",
                        ImageUrl = "https://hermes-application-images.s3.eu-central-1.amazonaws.com/fashion.png",
                        IsDeleted = false,
                        Name = "Moda & Aksesuarlar",
                        Slug = "fashion-and-accessories"
                    },
                    new
                    {
                        Id = 6L,
                        Description = "Film & Kitap Eşyaları",
                        ImageUrl = "https://hermes-application-images.s3.eu-central-1.amazonaws.com/book.png",
                        IsDeleted = false,
                        Name = "Film & Kitap",
                        Slug = "movies-and-books"
                    },
                    new
                    {
                        Id = 7L,
                        Description = "Diğer Eşyalar",
                        ImageUrl = "https://hermes-application-images.s3.eu-central-1.amazonaws.com/other.png",
                        IsDeleted = false,
                        Name = "Diğer",
                        Slug = "others"
                    });
            });
#pragma warning restore 612, 618
        }
    }
}