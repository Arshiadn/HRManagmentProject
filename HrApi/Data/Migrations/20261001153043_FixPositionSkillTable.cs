using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApi.Migrations
{
    /// <inheritdoc />
    public partial class FixPositionSkillTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PositionSkills_Positions_PositionId1",
                table: "PositionSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_PositionSkills_Skills_SkillId1",
                table: "PositionSkills");

            migrationBuilder.DropIndex(
                name: "IX_PositionSkills_PositionId1",
                table: "PositionSkills");

            migrationBuilder.DropIndex(
                name: "IX_PositionSkills_SkillId1",
                table: "PositionSkills");

            migrationBuilder.DropColumn(
                name: "PositionId1",
                table: "PositionSkills");

            migrationBuilder.DropColumn(
                name: "SkillId1",
                table: "PositionSkills");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionId1",
                table: "PositionSkills",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkillId1",
                table: "PositionSkills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PositionSkills_PositionId1",
                table: "PositionSkills",
                column: "PositionId1");

            migrationBuilder.CreateIndex(
                name: "IX_PositionSkills_SkillId1",
                table: "PositionSkills",
                column: "SkillId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PositionSkills_Positions_PositionId1",
                table: "PositionSkills",
                column: "PositionId1",
                principalTable: "Positions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PositionSkills_Skills_SkillId1",
                table: "PositionSkills",
                column: "SkillId1",
                principalTable: "Skills",
                principalColumn: "Id");
        }
    }
}
