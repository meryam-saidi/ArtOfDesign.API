using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtOfDesign.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDuplicateCuisineRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaracteristiquesTechniquesCuisines_Cuisines_CuisineId1",
                table: "CaracteristiquesTechniquesCuisines");

            migrationBuilder.DropIndex(
                name: "IX_CaracteristiquesTechniquesCuisines_CuisineId1",
                table: "CaracteristiquesTechniquesCuisines");

            migrationBuilder.DropColumn(
                name: "CuisineId1",
                table: "CaracteristiquesTechniquesCuisines");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CuisineId1",
                table: "CaracteristiquesTechniquesCuisines",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CaracteristiquesTechniquesCuisines_CuisineId1",
                table: "CaracteristiquesTechniquesCuisines",
                column: "CuisineId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CaracteristiquesTechniquesCuisines_Cuisines_CuisineId1",
                table: "CaracteristiquesTechniquesCuisines",
                column: "CuisineId1",
                principalTable: "Cuisines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
