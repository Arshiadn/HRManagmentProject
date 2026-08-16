using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApi.Migrations
{
    /// <inheritdoc />
    public partial class DepartmentDescriptionAndFilteredIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Hr_Departments_Name",
                table: "Hr_Departments");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Hr_Departments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hr_Departments_Name",
                table: "Hr_Departments",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Hr_Departments_Name",
                table: "Hr_Departments");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Hr_Departments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hr_Departments_Name",
                table: "Hr_Departments",
                column: "Name",
                unique: true);
        }
    }
}
