using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlackOverload.Data.Migrations
{
    public partial class tookoutFunnyUserNum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostUserId",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "CommentUserId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "UserFunnyNumber",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "UserFunnyNumber",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuestionUserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserFunnyNumber",
                table: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostUserId",
                table: "Votes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommentUserId",
                table: "Replies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserFunnyNumber",
                table: "Replies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserFunnyNumber",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QuestionUserId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserFunnyNumber",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
