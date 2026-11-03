
namespace ArtOfDesign.Domain.Entities.Cuisines;

public class DimensionsCuisine
{
    public int Id { get; set; }

    public string HauteurCaissonBas { get; set; } = string.Empty;

    public string ProfondeurCaissonBas { get; set; } = string.Empty;

    public string HauteurCaissonHaut { get; set; } = string.Empty;

    public string ProfondeurCaissonHaut { get; set; } = string.Empty;

    public string CuisineId { get; set; } = string.Empty;

}
