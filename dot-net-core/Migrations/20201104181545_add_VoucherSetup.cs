using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class add_VoucherSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "VoucherSetup",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(nullable: true),
            //        Description = table.Column<string>(nullable: true),
            //        Terms = table.Column<string>(nullable: true),
            //        Limitations = table.Column<string>(nullable: true),
            //        Type = table.Column<int>(nullable: false),
            //        Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        IsMultipleTimeUsage = table.Column<string>(nullable: true),
            //        StartDate = table.Column<DateTime>(nullable: false),
            //        EndDate = table.Column<DateTime>(nullable: false),
            //        StoreId = table.Column<int>(nullable: false),
            //        Status = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: true),
            //        CreatedBy = table.Column<int>(nullable: false),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<int>(nullable: false),
            //        DeletedOn = table.Column<DateTime>(nullable: true),
            //        DeletedBy = table.Column<int>(nullable: false),
            //        IsDeleted = table.Column<bool>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_VoucherSetup", x => x.Id);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoucherSetup");
        }
    }
}
