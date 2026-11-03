using ArtOfDesign.Domain.Entities.Cuisines;

namespace ArtOfDesign.Domain.Entities;

public class Cuisine
{
    public string Id { get; set; } = string.Empty;

    public string Categorie { get; set; } = "Cuisines";

    public string Forme { get; set; } = string.Empty;

    public string Nom { get; set; } = string.Empty;

    public decimal PrixInitial { get; set; }

    public int Reduction { get; set; }

    public string Description { get; set; } = string.Empty;

    public string Nb { get; set; } = string.Empty;

    public DimensionsCuisine Dimensions { get; set; } = null!;

    public List<ImageCuisine> Images { get; set; } = new();

    public CaracteristiquesTechniquesCuisine CaracteristiquesTechniques
    { get; set; } = null!;

    public List<CuisineCouleur> CuisineCouleurs { get; set; } = new();
}
