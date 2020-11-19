using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Modify_SetStores_2808 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency_ID",
                table: "Set_Stores");

            migrationBuilder.DropColumn(
                name: "Store_ID",
                table: "Set_Stores");

            migrationBuilder.DropColumn(
                name: "Store_name",
                table: "Set_Stores");

            migrationBuilder.AddColumn<Guid>(
                name: "StoreGuid",
                table: "Set_Stores",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "StoreName",
                table: "Set_Stores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreGuid",
                table: "Set_Stores");

            migrationBuilder.DropColumn(
                name: "StoreName",
                table: "Set_Stores");

            migrationBuilder.AddColumn<int>(
                name: "Currency_ID",
                table: "Set_Stores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "Store_ID",
                table: "Set_Stores",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Store_name",
                table: "Set_Stores",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
