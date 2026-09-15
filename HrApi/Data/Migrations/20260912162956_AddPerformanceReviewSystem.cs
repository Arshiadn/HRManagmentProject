using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceReviewSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReviewPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StartsOn = table.Column<DateOnly>(type: "date", nullable: false),
                    EndsOn = table.Column<DateOnly>(type: "date", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewRubrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewRubrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewRubrics_ReviewPeriods_ReviewPeriodId",
                        column: x => x.ReviewPeriodId,
                        principalTable: "ReviewPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PerformanceReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewPeriodId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewRubricId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewRubricId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeeComment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    SubmittedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    AcknowledgedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RubricSnapshotJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverallScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerformanceReviews_Hr_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Hr_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PerformanceReviews_ReviewPeriods_ReviewPeriodId",
                        column: x => x.ReviewPeriodId,
                        principalTable: "ReviewPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PerformanceReviews_ReviewPeriods_ReviewPeriodId1",
                        column: x => x.ReviewPeriodId1,
                        principalTable: "ReviewPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerformanceReviews_ReviewRubrics_ReviewRubricId",
                        column: x => x.ReviewRubricId,
                        principalTable: "ReviewRubrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PerformanceReviews_ReviewRubrics_ReviewRubricId1",
                        column: x => x.ReviewRubricId1,
                        principalTable: "ReviewRubrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RubricCriteria",
                columns: table => new
                {
                    ReviewRubricId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RubricCriteria", x => new { x.ReviewRubricId, x.Code });
                    table.ForeignKey(
                        name: "FK_RubricCriteria_ReviewRubrics_ReviewRubricId",
                        column: x => x.ReviewRubricId,
                        principalTable: "ReviewRubrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewAuditEntries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerformanceReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FromStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ToStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActorType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActorId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OccurredAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewAuditEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewAuditEntries_PerformanceReviews_PerformanceReviewId",
                        column: x => x.PerformanceReviewId,
                        principalTable: "PerformanceReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewEmployeeResponses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerformanceReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Acknowledged = table.Column<bool>(type: "bit", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RespondedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewEmployeeResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewEmployeeResponses_PerformanceReviews_PerformanceReviewId",
                        column: x => x.PerformanceReviewId,
                        principalTable: "PerformanceReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewScores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerformanceReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CriterionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewScores_PerformanceReviews_PerformanceReviewId",
                        column: x => x.PerformanceReviewId,
                        principalTable: "PerformanceReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_EmployeeId",
                table: "PerformanceReviews",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_ReviewPeriodId_EmployeeId",
                table: "PerformanceReviews",
                columns: new[] { "ReviewPeriodId", "EmployeeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_ReviewPeriodId1",
                table: "PerformanceReviews",
                column: "ReviewPeriodId1");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_ReviewRubricId",
                table: "PerformanceReviews",
                column: "ReviewRubricId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_ReviewRubricId1",
                table: "PerformanceReviews",
                column: "ReviewRubricId1");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewAuditEntries_PerformanceReviewId_OccurredAt",
                table: "ReviewAuditEntries",
                columns: new[] { "PerformanceReviewId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewEmployeeResponses_PerformanceReviewId",
                table: "ReviewEmployeeResponses",
                column: "PerformanceReviewId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewPeriods_EndsOn",
                table: "ReviewPeriods",
                column: "EndsOn");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewPeriods_StartsOn",
                table: "ReviewPeriods",
                column: "StartsOn");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewRubrics_ReviewPeriodId_Version",
                table: "ReviewRubrics",
                columns: new[] { "ReviewPeriodId", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewScores_PerformanceReviewId_CriterionCode",
                table: "ReviewScores",
                columns: new[] { "PerformanceReviewId", "CriterionCode" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewAuditEntries");

            migrationBuilder.DropTable(
                name: "ReviewEmployeeResponses");

            migrationBuilder.DropTable(
                name: "ReviewScores");

            migrationBuilder.DropTable(
                name: "RubricCriteria");

            migrationBuilder.DropTable(
                name: "PerformanceReviews");

            migrationBuilder.DropTable(
                name: "ReviewRubrics");

            migrationBuilder.DropTable(
                name: "ReviewPeriods");
        }
    }
}
