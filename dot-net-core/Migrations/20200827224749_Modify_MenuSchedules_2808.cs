using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Modify_MenuSchedules_2808 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Store_ID",
                table: "Emenu_MenuSchedules");

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Emenu_MenuSchedules",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Emenu_MenuSchedules");

            migrationBuilder.AddColumn<Guid>(
                name: "Store_ID",
                table: "Emenu_MenuSchedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
