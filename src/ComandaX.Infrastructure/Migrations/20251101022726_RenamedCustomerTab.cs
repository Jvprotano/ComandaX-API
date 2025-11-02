using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComandaX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamedCustomerTab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Tab_TabId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Tab");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TabId",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerTabId",
                table: "Orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerTabs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    TableId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTabs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerTabs_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerTabId",
                table: "Orders",
                column: "CustomerTabId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTabs_TableId",
                table: "CustomerTabs",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CustomerTabs_CustomerTabId",
                table: "Orders",
                column: "CustomerTabId",
                principalTable: "CustomerTabs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CustomerTabs_CustomerTabId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "CustomerTabs");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerTabId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerTabId",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "Tab",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TableId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tab", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tab_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TabId",
                table: "Orders",
                column: "TabId");

            migrationBuilder.CreateIndex(
                name: "IX_Tab_TableId",
                table: "Tab",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Tab_TabId",
                table: "Orders",
                column: "TabId",
                principalTable: "Tab",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
