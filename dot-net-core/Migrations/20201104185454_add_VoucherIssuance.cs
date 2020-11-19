using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class add_VoucherIssuance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VoucherIssuance",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherId = table.Column<int>(nullable: false),
                    RequestedDate = table.Column<DateTime>(nullable: true),
                    ApprovedDate = table.Column<DateTime>(nullable: true),
                    IssuedDate = table.Column<DateTime>(nullable: true),
                    StoreId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    Source = table.Column<int>(nullable: false),
                    UniqueId = table.Column<string>(nullable: true),
                    CampaignText = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherIssuance", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoucherIssuance");
        }
    }
}
