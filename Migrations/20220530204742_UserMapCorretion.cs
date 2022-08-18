using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApi.Migrations
{
    public partial class UserMapCorretion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "User",
                type: "VARCHAR(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "GitHub",
                table: "User",
                type: "VARCHAR(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "NVARCHAR(160)",
                maxLength: 160,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(120)",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "User",
                type: "Text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "User",
                type: "VARCHAR(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GitHub",
                table: "User",
                type: "VARCHAR(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "NVARCHAR(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(160)",
                oldMaxLength: 160);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "User",
                type: "Text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "Text",
                oldNullable: true);
        }
    }
}
