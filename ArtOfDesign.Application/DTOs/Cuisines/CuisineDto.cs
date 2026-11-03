namespace ArtOfDesign.Application.DTOs.Cuisines;

public class CuisineDto
{
    public string Id { get; set; } = string.Empty;
    public string Categorie { get; set; } = string.Empty;
    public string Forme { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public decimal PrixInitial { get; set; }
    public int Reduction { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Nb { get; set; } = string.Empty;

    public DimensionsCuisineDto Dimensions { get; set; } = null!;
    public List<ImageCuisineDto> Images { get; set; } = new();
    public CaracteristiquesTechniquesCuisineDto CaracteristiquesTechniques
    { get; set; } = null!;
    public List<CouleurCuisineDto> Couleurs { get; set; } = new();
}