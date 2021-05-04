using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hermes.API.Advertisement.Migrations
{
    public partial class AddApplicationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvertisementApplications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstimatedBarrowDays = table.Column<int>(type: "int", nullable: false),
                    ApplicantId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false,
                        defaultValueSql: "getutcdate()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table => { table.PrimaryKey("PK_AdvertisementApplications", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertisementApplications");
        }
    }
}