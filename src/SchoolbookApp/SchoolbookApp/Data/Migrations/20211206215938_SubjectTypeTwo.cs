using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolbookApp.Data.Migrations
{
    public partial class SubjectTypeTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubjectTypeId",
                table: "Subject",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubjectType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subject_SubjectTypeId",
                table: "Subject",
                column: "SubjectTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_SubjectType_SubjectTypeId",
                table: "Subject",
                column: "SubjectTypeId",
                principalTable: "SubjectType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subject_SubjectType_SubjectTypeId",
                table: "Subject");

            migrationBuilder.DropTable(
                name: "SubjectType");

            migrationBuilder.DropIndex(
                name: "IX_Subject_SubjectTypeId",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "SubjectTypeId",
                table: "Subject");
        }
    }
}
