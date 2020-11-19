using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Modify_Set_CountryCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Set_CountryCodes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Set_CountryCodes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Set_CountryCodes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Set_CountryCodes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Set_CountryCodes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "Set_CountryCodes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Set_CountryCodes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Set_CountryCodes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Set_CountryCodes");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Set_CountryCodes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Set_CountryCodes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Set_CountryCodes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Set_CountryCodes");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Set_CountryCodes");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Set_CountryCodes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Set_CountryCodes");
        }
    }
}
