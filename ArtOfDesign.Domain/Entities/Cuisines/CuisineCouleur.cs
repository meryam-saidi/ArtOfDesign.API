using ArtOfDesign.Domain.Entities.Catalogue;

namespace ArtOfDesign.Domain.Entities.Cuisines;

public class CuisineCouleur
{
    public string CuisineId { get; set; } = string.Empty;

    public int CouleurId { get; set; }

    public Cuisine Cuisine { get; set; } = null!;

    public Couleur Couleur { get; set; } = null!;
}