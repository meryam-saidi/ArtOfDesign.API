using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtOfDesign.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCuisines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cuisines",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Categorie = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Forme = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PrixInitial = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Reduction = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Nb = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuisines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaracteristiquesTechniquesCuisines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompositionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Caisson = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Facade = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CuisineId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CuisineId1 = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaracteristiquesTechniquesCuisines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaracteristiquesTechniquesCuisines_Cuisines_CuisineId",
                        column: x => x.CuisineId,
                        principalTable: "Cuisines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CaracteristiquesTechniquesCuisines_Cuisines_CuisineId1",
                        column: x => x.CuisineId1,
                        principalTable: "Cuisines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CouleursCuisine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeCouleur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePresentationUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CuisineId = table.Column<string>(type: "nvarchar(20)", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "DimensionsCuisines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CuisineId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    HauteurCaissonBas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfondeurCaissonBas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HauteurCaissonHaut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfondeurCaissonHaut = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimensionsCuisines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DimensionsCuisines_Cuisines_CuisineId",
                        column: x => x.CuisineId,
                        principalTable: "Cuisines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ImagesCuisine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrdreAffichage = table.Column<int>(type: "int", nullable: false),
                    EstPrincipale = table.Column<bool>(type: "bit", nullable: false),
                    CuisineId = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesCuisine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagesCuisine_Cuisines_CuisineId",
                        column: x => x.CuisineId,
                        principalTable: "Cuisines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaracteristiquesTechniquesCuisines_CuisineId",
                table: "CaracteristiquesTechniquesCuisines",
                column: "CuisineId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaracteristiquesTechniquesCuisines_CuisineId1",
                table: "CaracteristiquesTechniquesCuisines",
                column: "CuisineId1");

            migrationBuilder.CreateIndex(
                name: "IX_CouleursCuisine_CuisineId",
                table: "CouleursCuisine",
                column: "CuisineId");

            migrationBuilder.CreateIndex(
                name: "IX_DimensionsCuisines_CuisineId",
                table: "DimensionsCuisines",
                column: "CuisineId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImagesCuisine_CuisineId",
                table: "ImagesCuisine",
                column: "CuisineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaracteristiquesTechniquesCuisines");

            migrationBuilder.DropTable(
                name: "CouleursCuisine");

            migrationBuilder.DropTable(
                name: "DimensionsCuisines");

            migrationBuilder.DropTable(
                name: "ImagesCuisine");

            migrationBuilder.DropTable(
                name: "Cuisines");
        }
    }
}
