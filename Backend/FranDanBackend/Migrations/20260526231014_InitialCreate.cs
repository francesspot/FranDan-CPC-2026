using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FranDanBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Verifiers",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    verified = table.Column<bool>(type: "INTEGER", nullable: false),
                    code = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verifiers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    passwordHash = table.Column<string>(type: "TEXT", nullable: false),
                    email_notifications = table.Column<bool>(type: "INTEGER", nullable: false),
                    verifierId = table.Column<int>(type: "INTEGER", nullable: false),
                    occupation = table.Column<string>(type: "TEXT", nullable: false),
                    birthday = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                    table.ForeignKey(
                        name: "FK_Users_Verifiers_verifierId",
                        column: x => x.verifierId,
                        principalTable: "Verifiers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    friend1Id = table.Column<int>(type: "INTEGER", nullable: false),
                    friend2Id = table.Column<int>(type: "INTEGER", nullable: false),
                    accepted = table.Column<bool>(type: "INTEGER", nullable: false),
                    blacklisted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.id);
                    table.ForeignKey(
                        name: "FK_Friendships_Users_friend1Id",
                        column: x => x.friend1Id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friendships_Users_friend2Id",
                        column: x => x.friend2Id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    title = table.Column<string>(type: "TEXT", nullable: false),
                    category = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    startTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    admininstratorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.id);
                    table.ForeignKey(
                        name: "FK_Plans_Users_admininstratorId",
                        column: x => x.admininstratorId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Participations",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    userId = table.Column<int>(type: "INTEGER", nullable: false),
                    planId = table.Column<int>(type: "INTEGER", nullable: false),
                    accepted = table.Column<bool>(type: "INTEGER", nullable: false),
                    admin = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participations", x => x.id);
                    table.ForeignKey(
                        name: "FK_Participations_Plans_planId",
                        column: x => x.planId,
                        principalTable: "Plans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participations_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_friend1Id",
                table: "Friendships",
                column: "friend1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_friend2Id",
                table: "Friendships",
                column: "friend2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_planId",
                table: "Participations",
                column: "planId");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_userId",
                table: "Participations",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_admininstratorId",
                table: "Plans",
                column: "admininstratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_verifierId",
                table: "Users",
                column: "verifierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "Participations");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Verifiers");
        }
    }
}
