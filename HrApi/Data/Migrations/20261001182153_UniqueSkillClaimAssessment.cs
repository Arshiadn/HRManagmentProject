using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApi.Migrations
{
    /// <inheritdoc />
    public partial class UniqueSkillClaimAssessment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SkillAssessments_SkillClaimId",
                table: "SkillAssessments");

            migrationBuilder.RenameColumn(
                name: "OldExpiredAt",
                table: "EvidenceHistories",
                newName: "OldExpiresAt");

            migrationBuilder.RenameColumn(
                name: "NewExpiredAt",
                table: "EvidenceHistories",
                newName: "NewExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_SkillAssessments_SkillClaimId",
                table: "SkillAssessments",
                column: "SkillClaimId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SkillAssessments_SkillClaimId",
                table: "SkillAssessments");

            migrationBuilder.RenameColumn(
                name: "OldExpiresAt",
                table: "EvidenceHistories",
                newName: "OldExpiredAt");

            migrationBuilder.RenameColumn(
                name: "NewExpiresAt",
                table: "EvidenceHistories",
                newName: "NewExpiredAt");

            migrationBuilder.CreateIndex(
                name: "IX_SkillAssessments_SkillClaimId",
                table: "SkillAssessments",
                column: "SkillClaimId");
        }
    }
}
