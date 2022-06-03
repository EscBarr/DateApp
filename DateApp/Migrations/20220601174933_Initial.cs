using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DateApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompletedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Usedservices = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Degree = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "User_Couples",
                columns: table => new
                {
                    FirstUserId = table.Column<int>(type: "integer", nullable: false),
                    SecondUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Couples", x => new { x.FirstUserId, x.SecondUserId });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PwHash = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true),
                    Patronymic = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<bool>(type: "boolean", nullable: true),
                    Birthday = table.Column<DateTime>(type: "date", nullable: true),
                    UserRole = table.Column<string>(type: "text", nullable: true),
                    ReligionType = table.Column<string>(type: "text", nullable: true),
                    EducationId = table.Column<int>(type: "integer", nullable: true),
                    Avatar = table.Column<byte[]>(type: "bytea", nullable: true),
                    Height = table.Column<int>(type: "integer", nullable: true),
                    Weight = table.Column<int>(type: "integer", nullable: true),
                    EyeColor = table.Column<string>(type: "text", nullable: true),
                    Adress = table.Column<string>(type: "text", nullable: true),
                    AboutMe = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    OpinionOnSmoking = table.Column<string>(type: "text", nullable: true),
                    OpinionOnAlcohol = table.Column<string>(type: "text", nullable: true),
                    ContactPhone = table.Column<string>(type: "text", nullable: true),
                    RecievedLikes = table.Column<List<string>>(type: "text[]", nullable: true),
                    SentLikes = table.Column<List<string>>(type: "text[]", nullable: true),
                    Is_Participating = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Educations_EducationId",
                        column: x => x.EducationId,
                        principalTable: "Educations",
                        principalColumn: "Id");
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

            migrationBuilder.CreateIndex(
                name: "IX_Users_EducationId",
                table: "Users",
                column: "EducationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedServices");

            migrationBuilder.DropTable(
                name: "ListAllServices");

            migrationBuilder.DropTable(
                name: "User_Couples");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Educations");
        }
    }
}
