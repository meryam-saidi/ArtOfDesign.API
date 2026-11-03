namespace ArtOfDesign.Application.DTOs.Couleurs;

public class CouleurDto
{
    public int Id { get; set; }

    public string Nom { get; set; } = string.Empty;

    public string CodeCouleur { get; set; } = string.Empty;

    public string ImagePresentationUrl { get; set; } = string.Empty;
}