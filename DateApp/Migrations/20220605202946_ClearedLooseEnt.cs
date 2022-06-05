using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DateApp.Migrations
{
    public partial class ClearedLooseEnt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedServices");

            migrationBuilder.DropTable(
                name: "ListAllServices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompletedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Usedservices = table.Column<string>(type: "jsonb", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListAllServices",
                columns: table => new
                {
                    Services = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedServices_Id",
                table: "CompletedServices",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompletedServices_UserId",
                table: "CompletedServices",
                column: "UserId",
                unique: true);
        }
    }
}
