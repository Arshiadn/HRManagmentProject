using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApi.Migrations
{
    /// <inheritdoc />
    public partial class RefactorReviewRubricRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewRubrics_ReviewPeriods_ReviewPeriodId",
                table: "ReviewRubrics");

            migrationBuilder.DropTable(
                name: "ReviewEmployeeResponses");

            migrationBuilder.DropIndex(
                name: "IX_ReviewRubrics_ReviewPeriodId_Version",
                table: "ReviewRubrics");

            migrationBuilder.DropColumn(
                name: "ReviewPeriodId",
                table: "ReviewRubrics");

            migrationBuilder.AddColumn<Guid>(
                name: "SelectedRubricId",
                table: "ReviewPeriods",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewRubrics_Name_Version",
                table: "ReviewRubrics",
                columns: new[] { "Name", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewPeriods_SelectedRubricId",
                table: "ReviewPeriods",
                column: "SelectedRubricId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewPeriods_ReviewRubrics_SelectedRubricId",
                table: "ReviewPeriods",
                column: "SelectedRubricId",
                principalTable: "ReviewRubrics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewPeriods_ReviewRubrics_SelectedRubricId",
                table: "ReviewPeriods");

            migrationBuilder.DropIndex(
                name: "IX_ReviewRubrics_Name_Version",
                table: "ReviewRubrics");

            migrationBuilder.DropIndex(
                name: "IX_ReviewPeriods_SelectedRubricId",
                table: "ReviewPeriods");

            migrationBuilder.DropColumn(
                name: "SelectedRubricId",
                table: "ReviewPeriods");

            migrationBuilder.AddColumn<Guid>(
                name: "ReviewPeriodId",
                table: "ReviewRubrics",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ReviewEmployeeResponses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Acknowledged = table.Column<bool>(type: "bit", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PerformanceReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_ReviewRubrics_ReviewPeriodId_Version",
                table: "ReviewRubrics",
                columns: new[] { "ReviewPeriodId", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewEmployeeResponses_PerformanceReviewId",
                table: "ReviewEmployeeResponses",
                column: "PerformanceReviewId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewRubrics_ReviewPeriods_ReviewPeriodId",
                table: "ReviewRubrics",
                column: "ReviewPeriodId",
                principalTable: "ReviewPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
