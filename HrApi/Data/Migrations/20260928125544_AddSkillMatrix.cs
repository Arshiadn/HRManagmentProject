using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSkillMatrix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSkillStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    CurrentLevel = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSkillStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeSkillStates_Hr_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Hr_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeSkillStates_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PositionSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    RequiredLevel = table.Column<int>(type: "int", nullable: false),
                    PositionId1 = table.Column<int>(type: "int", nullable: true),
                    SkillId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionSkills_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PositionSkills_Positions_PositionId1",
                        column: x => x.PositionId1,
                        principalTable: "Positions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PositionSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PositionSkills_Skills_SkillId1",
                        column: x => x.SkillId1,
                        principalTable: "Skills",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SkillAssessments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    Decision = table.Column<int>(type: "int", nullable: false),
                    AssessorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssessedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillAssessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillAssessments_Hr_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Hr_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SkillAssessments_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SkillEvidences",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillEvidences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillEvidences_Hr_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Hr_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SkillEvidences_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SkillStateHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    FromLevel = table.Column<int>(type: "int", nullable: true),
                    ToLevel = table.Column<int>(type: "int", nullable: false),
                    SkillAssessmentId = table.Column<long>(type: "bigint", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillStateHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillStateHistories_Hr_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Hr_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SkillStateHistories_SkillAssessments_SkillAssessmentId",
                        column: x => x.SkillAssessmentId,
                        principalTable: "SkillAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SkillStateHistories_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvidenceHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillEvidenceId = table.Column<long>(type: "bigint", nullable: false),
                    OldIssuedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NewIssuedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OldExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NewExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OldType = table.Column<int>(type: "int", nullable: true),
                    NewType = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ChangedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvidenceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvidenceHistories_SkillEvidences_SkillEvidenceId",
                        column: x => x.SkillEvidenceId,
                        principalTable: "SkillEvidences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkillStates_EmployeeId",
                table: "EmployeeSkillStates",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSkillStates_SkillId_EmployeeId",
                table: "EmployeeSkillStates",
                columns: new[] { "SkillId", "EmployeeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceHistories_SkillEvidenceId",
                table: "EvidenceHistories",
                column: "SkillEvidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionSkills_PositionId_SkillId",
                table: "PositionSkills",
                columns: new[] { "PositionId", "SkillId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PositionSkills_PositionId1",
                table: "PositionSkills",
                column: "PositionId1");

            migrationBuilder.CreateIndex(
                name: "IX_PositionSkills_SkillId",
                table: "PositionSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionSkills_SkillId1",
                table: "PositionSkills",
                column: "SkillId1");

            migrationBuilder.CreateIndex(
                name: "IX_SkillAssessments_EmployeeId_SkillId",
                table: "SkillAssessments",
                columns: new[] { "EmployeeId", "SkillId" });

            migrationBuilder.CreateIndex(
                name: "IX_SkillAssessments_SkillId",
                table: "SkillAssessments",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillEvidences_EmployeeId",
                table: "SkillEvidences",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillEvidences_SkillId_EmployeeId",
                table: "SkillEvidences",
                columns: new[] { "SkillId", "EmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Title",
                table: "Skills",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillStateHistories_EmployeeId_SkillId",
                table: "SkillStateHistories",
                columns: new[] { "EmployeeId", "SkillId" });

            migrationBuilder.CreateIndex(
                name: "IX_SkillStateHistories_SkillAssessmentId",
                table: "SkillStateHistories",
                column: "SkillAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillStateHistories_SkillId",
                table: "SkillStateHistories",
                column: "SkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeSkillStates");

            migrationBuilder.DropTable(
                name: "EvidenceHistories");

            migrationBuilder.DropTable(
                name: "PositionSkills");

            migrationBuilder.DropTable(
                name: "SkillStateHistories");

            migrationBuilder.DropTable(
                name: "SkillEvidences");

            migrationBuilder.DropTable(
                name: "SkillAssessments");

            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
