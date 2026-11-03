
using ArtOfDesign.Domain.Entities;

namespace ArtOfDesign.Domain.Entities.Cuisines;

public class ImageCuisine
{
    public int Id { get; set; }

    public string Url { get; set; } = string.Empty;

    public string PublicId { get; set; } = string.Empty;

    public int OrdreAffichage { get; set; }

    public bool EstPrincipale { get; set; }

    public string CuisineId { get; set; } = string.Empty;
}
