using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionSkillHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PositionSkillHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionSkillId = table.Column<int>(type: "int", nullable: false),
                    OldRequiredLevel = table.Column<int>(type: "int", nullable: false),
                    NewRequiredLevel = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionSkillHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionSkillHistories_PositionSkills_PositionSkillId",
                        column: x => x.PositionSkillId,
                        principalTable: "PositionSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PositionSkillHistories_PositionSkillId",
                table: "PositionSkillHistories",
                column: "PositionSkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PositionSkillHistories");
        }
    }
}
