namespace ArtOfDesign.Application.DTOs.Cuisines;

public class UpdateCuisineDto
{
    public string Categorie { get; set; } = "Cuisines";
    public string Forme { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public decimal PrixInitial { get; set; }
    public int Reduction { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Nb { get; set; } = string.Empty;

    public DimensionsCuisineDto? Dimensions { get; set; } = new();
    public CaracteristiquesTechniquesCuisineDto? CaracteristiquesTechniques { get; set; } = new();

    public List<ImageCuisineDto> Images { get; set; } = new(); //les nouveaux fichiers seront reçus séparément par le contrôleur.
    
    public List<int> CouleurIds { get; set; } = new();
}