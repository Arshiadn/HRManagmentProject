using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApi.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeSkillClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkillAssessments_Hr_Employees_EmployeeId",
                table: "SkillAssessments");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillAssessments_Skills_SkillId",
                table: "SkillAssessments");

            migrationBuilder.DropIndex(
                name: "IX_SkillAssessments_EmployeeId_SkillId",
                table: "SkillAssessments");

            migrationBuilder.DropIndex(
                name: "IX_SkillAssessments_SkillId",
                table: "SkillAssessments");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "SkillAssessments");

            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "SkillAssessments");

            migrationBuilder.AddColumn<long>(
                name: "SkillClaimId",
                table: "SkillAssessments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "EmployeeSkillClaims",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    SkillEvidenceId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimedLevel = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSkillClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeSkillClaims_Hr_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Hr_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeSkillClaims_SkillEvidences_SkillEvidenceId",
                        column: x => x.SkillEvidenceId,
                        principalTable: "SkillEvidences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeSkillClaims_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillAssessments_SkillClaimId",
                table: "SkillAssessments",
                column: "SkillClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkillClaims_EmployeeId_SkillId",
                table: "EmployeeSkillClaims",
                columns: new[] { "EmployeeId", "SkillId" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkillClaims_SkillEvidenceId",
                table: "EmployeeSkillClaims",
                column: "SkillEvidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkillClaims_SkillId",
                table: "EmployeeSkillClaims",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillAssessments_EmployeeSkillClaims_SkillClaimId",
                table: "SkillAssessments",
                column: "SkillClaimId",
                principalTable: "EmployeeSkillClaims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkillAssessments_EmployeeSkillClaims_SkillClaimId",
                table: "SkillAssessments");

            migrationBuilder.DropTable(
                name: "EmployeeSkillClaims");

            migrationBuilder.DropIndex(
                name: "IX_SkillAssessments_SkillClaimId",
                table: "SkillAssessments");

            migrationBuilder.DropColumn(
                name: "SkillClaimId",
                table: "SkillAssessments");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "SkillAssessments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SkillId",
                table: "SkillAssessments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SkillAssessments_EmployeeId_SkillId",
                table: "SkillAssessments",
                columns: new[] { "EmployeeId", "SkillId" });

            migrationBuilder.CreateIndex(
                name: "IX_SkillAssessments_SkillId",
                table: "SkillAssessments",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillAssessments_Hr_Employees_EmployeeId",
                table: "SkillAssessments",
                column: "EmployeeId",
                principalTable: "Hr_Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillAssessments_Skills_SkillId",
                table: "SkillAssessments",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
