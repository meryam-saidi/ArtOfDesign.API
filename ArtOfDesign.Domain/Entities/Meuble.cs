using ArtOfDesign.Domain.Entities.Meubles;


namespace ArtOfDesign.Domain.Entities;

public class Meuble
{
    public string Id { get; set; } = string.Empty;
    public string Categorie { get; set; } = string.Empty;
    public string Forme { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public decimal PrixInitial { get; set; }
    public int Reduction { get; set; }
    public string Description { get; set; } = string.Empty;

    public DimensionsMeuble Dimensions { get; set; } = null!;
    public List<ImageMeuble> Images { get; set; } = new();
    public CaracteristiquesTechniquesMeuble CaracteristiquesTechniques
    { get; set; } = null!;
    public List<MeubleCouleur> MeubleCouleurs { get; set; } = new();
}
