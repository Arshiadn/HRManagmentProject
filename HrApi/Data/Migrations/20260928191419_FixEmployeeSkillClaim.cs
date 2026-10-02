using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApi.Migrations
{
    /// <inheritdoc />
    public partial class FixEmployeeSkillClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSkillClaims_SkillEvidences_SkillEvidenceId",
                table: "EmployeeSkillClaims");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSkillClaims_SkillEvidenceId",
                table: "EmployeeSkillClaims");

            migrationBuilder.DropColumn(
                name: "SkillEvidenceId",
                table: "EmployeeSkillClaims");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EmployeeSkillClaims",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EmployeeSkillClaimEvidence",
                columns: table => new
                {
                    EmployeeSkillClaimId = table.Column<long>(type: "bigint", nullable: false),
                    SkillEvidenceId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSkillClaimEvidence", x => new { x.EmployeeSkillClaimId, x.SkillEvidenceId });
                    table.ForeignKey(
                        name: "FK_EmployeeSkillClaimEvidence_EmployeeSkillClaims_EmployeeSkillClaimId",
                        column: x => x.EmployeeSkillClaimId,
                        principalTable: "EmployeeSkillClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeSkillClaimEvidence_SkillEvidences_SkillEvidenceId",
                        column: x => x.SkillEvidenceId,
                        principalTable: "SkillEvidences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkillClaimEvidence_SkillEvidenceId",
                table: "EmployeeSkillClaimEvidence",
                column: "SkillEvidenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeSkillClaimEvidence");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EmployeeSkillClaims");

            migrationBuilder.AddColumn<long>(
                name: "SkillEvidenceId",
                table: "EmployeeSkillClaims",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkillClaims_SkillEvidenceId",
                table: "EmployeeSkillClaims",
                column: "SkillEvidenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSkillClaims_SkillEvidences_SkillEvidenceId",
                table: "EmployeeSkillClaims",
                column: "SkillEvidenceId",
                principalTable: "SkillEvidences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
