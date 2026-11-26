
namespace ArtOfDesign.Domain.Entities.Projets;

public class ImageProjet
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string PublicId { get; set; } = string.Empty;
    public bool Est3D { get; set; } // true = image3d (Conception 3D), false = image (Réalisation réelle)
    public int OrdreAffichage { get; set; }
    public bool EstPrincipale { get; set; }

    public string ProjetId { get; set; } = string.Empty;
} 