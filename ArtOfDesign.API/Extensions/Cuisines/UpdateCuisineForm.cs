using ArtOfDesign.Application.DTOs.Cuisines;

namespace ArtOfDesign.API.Extensions.Cuisines;

public class UpdateCuisineForm
{
    public string Categorie { get; set; } = string.Empty;
    public string Forme { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public decimal PrixInitial { get; set; }
    public int Reduction { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Nb { get; set; } = string.Empty;

    // Dimensions
    public string HauteurCaissonBas { get; set; } = string.Empty;
    public string ProfondeurCaissonBas { get; set; } = string.Empty;
    public string HauteurCaissonHaut { get; set; } = string.Empty;
    public string ProfondeurCaissonHaut { get; set; } = string.Empty;

    // Caractéristiques techniques
    public List<string> CompositionType { get; set; } = new();
    public string Caisson { get; set; } = string.Empty;
    public string Facade { get; set; } = string.Empty;

    // Couleurs
    public List<int> CouleurIds { get; set; } = new();

    // Images envoyées depuis le PC
    public List<IFormFile> Images { get; set; } = new();
}
