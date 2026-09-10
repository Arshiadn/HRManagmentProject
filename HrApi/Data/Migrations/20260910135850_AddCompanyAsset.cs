using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAssets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetAssignments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ReturnedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    AssignmentNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReturnNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetAssignments_CompanyAssets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "CompanyAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetAssignments_Hr_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Hr_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignments_AssetId_AssignedAt",
                table: "AssetAssignments",
                columns: new[] { "AssetId", "AssignedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignments_EmployeeId",
                table: "AssetAssignments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAssets_AssetCode",
                table: "CompanyAssets",
                column: "AssetCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAssets_SerialNumber",
                table: "CompanyAssets",
                column: "SerialNumber",
                unique: true,
                filter: "[SerialNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetAssignments");

            migrationBuilder.DropTable(
                name: "CompanyAssets");
        }
    }
}
