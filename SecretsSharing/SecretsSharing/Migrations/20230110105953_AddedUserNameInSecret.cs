using Microsoft.EntityFrameworkCore.Migrations;

namespace SecretsSharing.Migrations
{
    public partial class AddedUserNameInSecret : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TextSecrets",
                table: "TextSecrets");

            migrationBuilder.RenameTable(
                name: "TextSecrets",
                newName: "Secrets");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Secrets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Secrets",
                table: "Secrets",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Secrets",
                table: "Secrets");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Secrets");

            migrationBuilder.RenameTable(
                name: "Secrets",
                newName: "TextSecrets");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextSecrets",
                table: "TextSecrets",
                column: "Id");
        }
    }
}
