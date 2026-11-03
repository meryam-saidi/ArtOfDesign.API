namespace ArtOfDesign.Application.DTOs.Couleurs;

public class CreateCouleurDto
{
    public string Nom { get; set; } = string.Empty;

    public string CodeCouleur { get; set; } = string.Empty;

    /*public string ImagePresentationUrl { get; set; } = string.Empty;*/
    //Il ne doit pas contenir ImagePresentationUrl, puisque cette URL est générée par Cloudinary après le téléversement.
}