namespace ArtOfDesign.API.Extensions;

public class UpdateCouleurForm
{
    public string Nom { get; set; } = string.Empty;

    public string CodeCouleur { get; set; } = string.Empty;

    public IFormFile? Image { get; set; }
}
