using Microsoft.EntityFrameworkCore.Migrations;

namespace final.Migrations
{
    public partial class add_isAdmin_column_To_TEACHER_ENTITY : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isAdmin",
                table: "Teachers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAdmin",
                table: "Teachers");
        }
    }
}
