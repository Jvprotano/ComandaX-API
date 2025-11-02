using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComandaX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NeedPreparationProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeedPreparation",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedPreparation",
                table: "Products");
        }
    }
}
