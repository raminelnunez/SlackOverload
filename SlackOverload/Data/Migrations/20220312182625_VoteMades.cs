using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlackOverload.Data.Migrations
{
    public partial class VoteMades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUpvote",
                table: "Votes");

            migrationBuilder.AddColumn<string>(
                name: "VoteMade",
                table: "Votes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoteMade",
                table: "Votes");

            migrationBuilder.AddColumn<bool>(
                name: "IsUpvote",
                table: "Votes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
