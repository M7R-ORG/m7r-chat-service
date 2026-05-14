using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameAttachmentContentToFileId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Attachments",
                newName: "FileId");

            migrationBuilder.Sql("DELETE FROM \"Attachments\";");
            migrationBuilder.Sql("UPDATE \"Accounts\" SET \"Image\" = NULL;");
            migrationBuilder.Sql("UPDATE \"Channels\" SET \"Image\" = NULL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "Attachments",
                newName: "Content");
        }
    }
}
