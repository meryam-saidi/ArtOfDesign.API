using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtOfDesign.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMeubles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meubles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Categorie = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Forme = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PrixInitial = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    Reduction = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meubles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaracteristiquesTechniquesMeubles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompositionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Caisson = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Facade = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MeubleId = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaracteristiquesTechniquesMeubles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaracteristiquesTechniquesMeubles_Meubles_MeubleId",
                        column: x => x.MeubleId,
                        principalTable: "Meubles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DimensionsMeubles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Largeur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hauteur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profondeur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeubleId = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimensionsMeubles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DimensionsMeubles_Meubles_MeubleId",
                        column: x => x.MeubleId,
                        principalTable: "Meubles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImagesMeuble",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrdreAffichage = table.Column<int>(type: "int", nullable: false),
                    EstPrincipale = table.Column<bool>(type: "bit", nullable: false),
                    MeubleId = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesMeuble", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagesMeuble_Meubles_MeubleId",
                        column: x => x.MeubleId,
                        principalTable: "Meubles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeubleCouleurs",
                columns: table => new
                {
                    MeubleId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CouleurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeubleCouleurs", x => new { x.MeubleId, x.CouleurId });
                    table.ForeignKey(
                        name: "FK_MeubleCouleurs_Couleurs_CouleurId",
                        column: x => x.CouleurId,
                        principalTable: "Couleurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeubleCouleurs_Meubles_MeubleId",
                        column: x => x.MeubleId,
                        principalTable: "Meubles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaracteristiquesTechniquesMeubles_MeubleId",
                table: "CaracteristiquesTechniquesMeubles",
                column: "MeubleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DimensionsMeubles_MeubleId",
                table: "DimensionsMeubles",
                column: "MeubleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImagesMeuble_MeubleId",
                table: "ImagesMeuble",
                column: "MeubleId");

            migrationBuilder.CreateIndex(
                name: "IX_MeubleCouleurs_CouleurId",
                table: "MeubleCouleurs",
                column: "CouleurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaracteristiquesTechniquesMeubles");

            migrationBuilder.DropTable(
                name: "DimensionsMeubles");

            migrationBuilder.DropTable(
                name: "ImagesMeuble");

            migrationBuilder.DropTable(
                name: "MeubleCouleurs");

            migrationBuilder.DropTable(
                name: "Meubles");
        }
    }
}
