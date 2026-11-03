using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtOfDesign.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GlobalColors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Couleurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CodeCouleur = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ImagePresentationUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Couleurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CuisineCouleurs",
                columns: table => new
                {
                    CuisineId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CouleurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuisineCouleurs", x => new { x.CuisineId, x.CouleurId });
                    table.ForeignKey(
                        name: "FK_CuisineCouleurs_Couleurs_CouleurId",
                        column: x => x.CouleurId,
                        principalTable: "Couleurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CuisineCouleurs_Cuisines_CuisineId",
                        column: x => x.CuisineId,
                        principalTable: "Cuisines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CuisineCouleurs_CouleurId",
                table: "CuisineCouleurs",
                column: "CouleurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CuisineCouleurs");

            migrationBuilder.DropTable(
                name: "Couleurs");

            migrationBuilder.CreateTable(
                name: "CouleursCuisine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CuisineId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CodeCouleur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePresentationUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouleursCuisine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouleursCuisine_Cuisines_CuisineId",
                        column: x => x.CuisineId,
                        principalTable: "Cuisines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CouleursCuisine_CuisineId",
                table: "CouleursCuisine",
                column: "CuisineId");
        }
    }
}
