using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DojoActivity.Migrations
{
    public partial class FirsMigra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersTable",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTable", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ActivityTable",
                columns: table => new
                {
                    ActivityId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    DurationForm = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTable", x => x.ActivityId);
                    table.ForeignKey(
                        name: "FK_ActivityTable_UsersTable_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersTable",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantTable",
                columns: table => new
                {
                    ParticipantId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantTable", x => x.ParticipantId);
                    table.ForeignKey(
                        name: "FK_ParticipantTable_ActivityTable_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "ActivityTable",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipantTable_UsersTable_UserId",
                        column: x => x.UserId,
                        principalTable: "UsersTable",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTable_UserId",
                table: "ActivityTable",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantTable_ActivityId",
                table: "ParticipantTable",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantTable_UserId",
                table: "ParticipantTable",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParticipantTable");

            migrationBuilder.DropTable(
                name: "ActivityTable");

            migrationBuilder.DropTable(
                name: "UsersTable");
        }
    }
}
