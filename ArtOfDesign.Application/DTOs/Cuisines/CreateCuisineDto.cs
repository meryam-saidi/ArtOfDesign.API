using System.Collections.Generic;

namespace ArtOfDesign.Application.DTOs.Cuisines;

public class CreateCuisineDto
{
    public string Id { get; set; } = string.Empty;
    public string Forme { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public decimal PrixInitial { get; set; }
    public int Reduction { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Nb { get; set; } = string.Empty;
    public DimensionsCuisineDto Dimensions { get; set; } = new();
    public CaracteristiquesTechniquesCuisineDto CaracteristiquesTechniques { get; set; } = new();
    public List<CreateCuisineImageDto> Images { get; set; } = new();
    public List<int> CouleurIds { get; set; } = new();
}
