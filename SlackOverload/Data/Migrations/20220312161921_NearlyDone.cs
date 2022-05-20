using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlackOverload.Data.Migrations
{
    public partial class NearlyDone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswerFunnyNumber",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswerFunnyNumber",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Questions");
        }
    }
}
