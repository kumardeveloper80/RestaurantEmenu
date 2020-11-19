using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Modify_Set_UserStores_2808 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Store_ID",
                table: "Sec_UserStores");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Sec_UserStores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Sec_UserStores",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Sec_UserStores");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Sec_UserStores");

            migrationBuilder.AddColumn<Guid>(
                name: "Store_ID",
                table: "Sec_UserStores",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
