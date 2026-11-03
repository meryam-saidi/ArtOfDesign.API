namespace ArtOfDesign.Domain.Entities.Catalogue;

public class Couleur
{
    public int Id { get; set; }

    public string Nom { get; set; } = string.Empty;

    public string CodeCouleur { get; set; } = string.Empty;

    public string ImagePresentationUrl { get; set; } = string.Empty;

    public string PublicId { get; set; } = string.Empty;
}