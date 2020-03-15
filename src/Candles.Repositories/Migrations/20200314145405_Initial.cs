using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Candles.Repositories.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "candles");

            migrationBuilder.CreateTable(
                name: "candles",
                schema: "candles",
                columns: table => new
                {
                    asset_pair_id = table.Column<string>(type: "varchar(36)", nullable: false),
                    time = table.Column<DateTime>(nullable: false),
                    type = table.Column<short>(nullable: false),
                    open = table.Column<double>(nullable: false),
                    close = table.Column<double>(nullable: false),
                    high = table.Column<double>(nullable: false),
                    low = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_candles", x => new { x.asset_pair_id, x.type, x.time });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "candles",
                schema: "candles");
        }
    }
}
