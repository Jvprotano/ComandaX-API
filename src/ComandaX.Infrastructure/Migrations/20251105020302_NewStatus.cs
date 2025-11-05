using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComandaX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TabId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "OrderProducts",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrderProducts");

            migrationBuilder.AddColumn<Guid>(
                name: "TabId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
