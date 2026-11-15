namespace ArtOfDesign.API.Extensions.Dressings;

public class UpdateDressingForm
{
    // Informations générales
    public string Categorie { get; set; } = string.Empty;
    public string Forme { get; set; } = string.Empty;
    public string TypeOuverture { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public decimal PrixInitial { get; set; }
    public int Reduction { get; set; }
    public string Description { get; set; } = string.Empty;

    // Dimensions
    public string Largeur { get; set; } = string.Empty;
    public string Hauteur { get; set; } = string.Empty;
    public string Profondeur { get; set; } = string.Empty;

    // Caractéristiques techniques
    public List<string> CompositionType { get; set; } = new();
    public string Caisson { get; set; } = string.Empty;
    public string Facade { get; set; } = string.Empty;

    // Couleurs
    public List<int> CouleurIds { get; set; } = new();

    // Images envoyées depuis le PC
    public List<IFormFile> Images { get; set; } = new();
}
