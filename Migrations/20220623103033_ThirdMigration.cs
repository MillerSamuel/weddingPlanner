using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace weddingPlanner.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Weddings_WeddingId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_WeddingId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WeddingId",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeddingId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_WeddingId",
                table: "Users",
                column: "WeddingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Weddings_WeddingId",
                table: "Users",
                column: "WeddingId",
                principalTable: "Weddings",
                principalColumn: "WeddingId");
        }
    }
}
