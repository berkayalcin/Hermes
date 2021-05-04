using Microsoft.EntityFrameworkCore.Migrations;

namespace Hermes.API.Advertisement.Migrations
{
    public partial class AddApplicationsTable4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EstimatedBarrowDays",
                table: "AdvertisementApplications",
                newName: "EstimatedBorrowDays");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EstimatedBorrowDays",
                table: "AdvertisementApplications",
                newName: "EstimatedBarrowDays");
        }
    }
}
