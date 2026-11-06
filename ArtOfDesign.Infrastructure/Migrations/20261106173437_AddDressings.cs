using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtOfDesign.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDressings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dressings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Categorie = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Forme = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TypeOuverture = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PrixInitial = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Reduction = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dressings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaracteristiquesTechniquesDressings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompositionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Caisson = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Facade = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DressingId = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaracteristiquesTechniquesDressings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaracteristiquesTechniquesDressings_Dressings_DressingId",
                        column: x => x.DressingId,
                        principalTable: "Dressings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DimensionsDressings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Largeur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hauteur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profondeur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DressingId = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimensionsDressings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DimensionsDressings_Dressings_DressingId",
                        column: x => x.DressingId,
                        principalTable: "Dressings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DressingCouleurs",
                columns: table => new
                {
                    DressingId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CouleurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DressingCouleurs", x => new { x.DressingId, x.CouleurId });
                    table.ForeignKey(
                        name: "FK_DressingCouleurs_Couleurs_CouleurId",
                        column: x => x.CouleurId,
                        principalTable: "Couleurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DressingCouleurs_Dressings_DressingId",
                        column: x => x.DressingId,
                        principalTable: "Dressings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImagesDressing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrdreAffichage = table.Column<int>(type: "int", nullable: false),
                    EstPrincipale = table.Column<bool>(type: "bit", nullable: false),
                    DressingId = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesDressing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagesDressing_Dressings_DressingId",
                        column: x => x.DressingId,
                        principalTable: "Dressings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaracteristiquesTechniquesDressings_DressingId",
                table: "CaracteristiquesTechniquesDressings",
                column: "DressingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DimensionsDressings_DressingId",
                table: "DimensionsDressings",
                column: "DressingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DressingCouleurs_CouleurId",
                table: "DressingCouleurs",
                column: "CouleurId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesDressing_DressingId",
                table: "ImagesDressing",
                column: "DressingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaracteristiquesTechniquesDressings");

            migrationBuilder.DropTable(
                name: "DimensionsDressings");

            migrationBuilder.DropTable(
                name: "DressingCouleurs");

            migrationBuilder.DropTable(
                name: "ImagesDressing");

            migrationBuilder.DropTable(
                name: "Dressings");
        }
    }
}
