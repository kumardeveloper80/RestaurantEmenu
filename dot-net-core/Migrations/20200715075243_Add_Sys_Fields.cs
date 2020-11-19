using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Add_Sys_Fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sys_Fields",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldID = table.Column<string>(nullable: true),
                    FormID = table.Column<string>(nullable: true),
                    DataType = table.Column<string>(nullable: true),
                    ModuleID = table.Column<int>(nullable: true),
                    SectionID = table.Column<string>(nullable: true),
                    Required = table.Column<bool>(nullable: true),
                    Visible = table.Column<bool>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DefaultValue = table.Column<string>(nullable: true),
                    Col = table.Column<int>(nullable: true),
                    MaxLength = table.Column<int>(nullable: true),
                    TextMode = table.Column<string>(nullable: true),
                    SourceTable = table.Column<string>(nullable: true),
                    SourceValue = table.Column<string>(nullable: true),
                    SourceText = table.Column<string>(nullable: true),
                    hasActive = table.Column<bool>(nullable: true),
                    sourcePageURL = table.Column<string>(nullable: true),
                    RequiredMessage = table.Column<string>(nullable: true),
                    RequiredPatternMessage = table.Column<string>(nullable: true),
                    RequiredPattern = table.Column<string>(nullable: true),
                    Readonly = table.Column<bool>(nullable: true),
                    Position = table.Column<int>(nullable: true),
                    Skin = table.Column<string>(nullable: true),
                    CSS = table.Column<string>(nullable: true),
                    CustomField = table.Column<bool>(nullable: true),
                    Incomplete = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Fields", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sys_Fields");
        }
    }
}
