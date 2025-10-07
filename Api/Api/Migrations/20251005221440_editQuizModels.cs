using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class editQuizModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizSubmissions_AspNetUsers_StudentId",
                table: "QuizSubmissions");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "QuizSubmissions");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "QuizSubmissions",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_QuizSubmissions_StudentId",
                table: "QuizSubmissions",
                newName: "IX_QuizSubmissions_UserId");

            migrationBuilder.AddColumn<string>(
                name: "CorrectOption",
                table: "QuizQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OptionA",
                table: "QuizQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OptionB",
                table: "QuizQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OptionC",
                table: "QuizQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OptionD",
                table: "QuizQuestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizSubmissions_AspNetUsers_UserId",
                table: "QuizSubmissions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizSubmissions_AspNetUsers_UserId",
                table: "QuizSubmissions");

            migrationBuilder.DropColumn(
                name: "CorrectOption",
                table: "QuizQuestions");

            migrationBuilder.DropColumn(
                name: "OptionA",
                table: "QuizQuestions");

            migrationBuilder.DropColumn(
                name: "OptionB",
                table: "QuizQuestions");

            migrationBuilder.DropColumn(
                name: "OptionC",
                table: "QuizQuestions");

            migrationBuilder.DropColumn(
                name: "OptionD",
                table: "QuizQuestions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "QuizSubmissions",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_QuizSubmissions_UserId",
                table: "QuizSubmissions",
                newName: "IX_QuizSubmissions_StudentId");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "QuizSubmissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_QuizSubmissions_AspNetUsers_StudentId",
                table: "QuizSubmissions",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
