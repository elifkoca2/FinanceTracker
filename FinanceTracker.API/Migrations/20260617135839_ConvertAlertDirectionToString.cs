using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class ConvertAlertDirectionToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AlertDirection",
                table: "WatchlistItems",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AlertDirection",
                table: "WatchlistItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
