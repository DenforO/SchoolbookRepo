using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolbookApp.Data.Migrations
{
    public partial class ReviewSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absence_SchoolClass_SchoolClassId",
                table: "Absence");

            migrationBuilder.RenameColumn(
                name: "SchoolClassId",
                table: "Absence",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Absence_SchoolClassId",
                table: "Absence",
                newName: "IX_Absence_SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absence_Subject_SubjectId",
                table: "Absence",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absence_Subject_SubjectId",
                table: "Absence");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Absence",
                newName: "SchoolClassId");

            migrationBuilder.RenameIndex(
                name: "IX_Absence_SubjectId",
                table: "Absence",
                newName: "IX_Absence_SchoolClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absence_SchoolClass_SchoolClassId",
                table: "Absence",
                column: "SchoolClassId",
                principalTable: "SchoolClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
