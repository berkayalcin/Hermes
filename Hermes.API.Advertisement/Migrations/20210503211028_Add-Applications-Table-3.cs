using Microsoft.EntityFrameworkCore.Migrations;

namespace Hermes.API.Advertisement.Migrations
{
    public partial class AddApplicationsTable3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "AdvertisementApplications",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "AdvertisementApplications");
        }
    }
}
