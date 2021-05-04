using Microsoft.EntityFrameworkCore.Migrations;

namespace Hermes.API.Catalog.Migrations
{
    public partial class AddInitialCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] {"Id", "Description", "ImageUrl", "Name", "Slug"},
                values: new object[,]
                {
                    {
                        1L, "Elektronik Eşyalar",
                        "https://hermes-application-images.s3.eu-central-1.amazonaws.com/laptop.png", "Elektronik",
                        "electronic"
                    },
                    {
                        2L, "Spor Outdoor Eşyaları",
                        "https://hermes-application-images.s3.eu-central-1.amazonaws.com/camp.png", "Spor & Outdoor",
                        "outdoor"
                    },
                    {
                        3L, "Motorsiklet, Bisiklet ve Arabalar",
                        "https://hermes-application-images.s3.eu-central-1.amazonaws.com/vehicle.png", "Araç",
                        "vehicles"
                    },
                    {
                        4L, "Ev & Bahçe Eşyaları",
                        "https://hermes-application-images.s3.eu-central-1.amazonaws.com/garden.png", "Ev & Bahçe",
                        "garden"
                    },
                    {
                        5L, "Moda ve Aksesuar Eşyaları",
                        "https://hermes-application-images.s3.eu-central-1.amazonaws.com/fashion.png",
                        "Moda & Aksesuarlar", "fashion-and-accessories"
                    },
                    {
                        6L, "Film & Kitap Eşyaları",
                        "https://hermes-application-images.s3.eu-central-1.amazonaws.com/book.png", "Film & Kitap",
                        "movies-and-books"
                    },
                    {
                        7L, "Diğer Eşyalar",
                        "https://hermes-application-images.s3.eu-central-1.amazonaws.com/other.png", "Diğer", "others"
                    }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7L);
        }
    }
}