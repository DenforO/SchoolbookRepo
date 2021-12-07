using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolbookApp.Data.Migrations
{
    public partial class SubjectChangeTypeFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_Subject_SubjectId",
                table: "Note");

            migrationBuilder.DropTable(
                name: "Absence");

            migrationBuilder.DropTable(
                name: "Grade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Note",
                table: "Note");

            migrationBuilder.RenameTable(
                name: "Note",
                newName: "Review");

            migrationBuilder.RenameIndex(
                name: "IX_Note_SubjectId",
                table: "Review",
                newName: "IX_Review_SubjectId");

            migrationBuilder.AddColumn<string>(
                name: "Basis",
                table: "Review",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Review",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Half",
                table: "Review",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinalGrade",
                table: "Review",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSemesterGrade",
                table: "Review",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Review",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isExcused",
                table: "Review",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Subject_SubjectId",
                table: "Review",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Subject_SubjectId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "Basis",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "Half",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "IsFinalGrade",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "IsSemesterGrade",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "isExcused",
                table: "Review");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Note");

            migrationBuilder.RenameIndex(
                name: "IX_Review_SubjectId",
                table: "Note",
                newName: "IX_Note_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Note",
                table: "Note",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Absence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Half = table.Column<bool>(type: "bit", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    isExcused = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Absence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Absence_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Basis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFinalGrade = table.Column<bool>(type: "bit", nullable: false),
                    IsSemesterGrade = table.Column<bool>(type: "bit", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grade_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absence_SubjectId",
                table: "Absence",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_SubjectId",
                table: "Grade",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Subject_SubjectId",
                table: "Note",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
