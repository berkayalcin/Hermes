using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hermes.API.Advertisement.Migrations
{
    public partial class AddApplicationsTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AdvertisementId",
                table: "AdvertisementApplications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvertisementId",
                table: "AdvertisementApplications");
        }
    }
}