
namespace ArtOfDesign.Domain.Entities.Cuisines;

public class CaracteristiquesTechniquesCuisine
{
    public int Id { get; set; }

    public List<string> CompositionType { get; set; } = new();

    public string Caisson { get; set; } = string.Empty;

    public string Facade { get; set; } = string.Empty;

    public string CuisineId { get; set; } = string.Empty;
}
