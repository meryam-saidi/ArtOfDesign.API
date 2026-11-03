using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtOfDesign.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePublicId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "ImagesCuisine",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "ImagesCuisine");
        }
    }
}
