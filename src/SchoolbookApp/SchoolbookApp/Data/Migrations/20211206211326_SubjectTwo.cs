using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolbookApp.Data.Migrations
{
    public partial class SubjectTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SchoolClassId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subject_SchoolClass_SchoolClassId",
                        column: x => x.SchoolClassId,
                        principalTable: "SchoolClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                       name: "FK_Subject_AspNetUsers_TeacherId",
                       column: x => x.TeacherId,
                       principalTable: "AspNetUsers",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subject_SchoolClassId",
                table: "Subject",
                column: "SchoolClassId");
            migrationBuilder.CreateIndex(
                name: "IX_Subject_TeacherId",
                table: "Subject",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subject");
        }
    }
}
