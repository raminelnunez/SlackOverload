using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlackOverload.Data.Migrations
{
    public partial class poop1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_UserId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Comments_CommentId",
                table: "Replies");

            migrationBuilder.AlterColumn<int>(
                name: "CommentId",
                table: "Replies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommentUserId",
                table: "Replies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommentUserName",
                table: "Replies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CommentUserReputation",
                table: "Replies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserFunnyNumber",
                table: "Replies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Replies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserReputation",
                table: "Replies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Questions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserFunnyNumber",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "QuestionFunnyNumber",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "QuestionUserId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QuestionUserName",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "QuestionUserReputation",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserFunnyNumber",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_UserId",
                table: "Questions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Comments_CommentId",
                table: "Replies",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_UserId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Comments_CommentId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "CommentUserId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "CommentUserName",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "CommentUserReputation",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "UserFunnyNumber",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "UserReputation",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "UserFunnyNumber",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuestionFunnyNumber",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "QuestionUserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "QuestionUserName",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "QuestionUserReputation",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserFunnyNumber",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "CommentId",
                table: "Replies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Questions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_UserId",
                table: "Questions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Comments_CommentId",
                table: "Replies",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }
    }
}
