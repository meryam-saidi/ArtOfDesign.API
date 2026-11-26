using System.ComponentModel.DataAnnotations;

namespace ArtOfDesign.API.Extensions.Projets;

public class ProjetImageForm
{
    public int? Id { get; set; }               // utilisé seulement en update
    public string? Title { get; set; }
    public bool? Est3D { get; set; }
    public IFormFile? File { get; set; }
    public int OrdreAffichage { get; set; }
    public bool EstPrincipale { get; set; }
}

public class ProjetForm
{
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ProjetImageForm> Images { get; set; } = new();
}

public class ProjetCreateForm
{
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<IFormFile> Images3D { get; set; } = new();
    public List<IFormFile> ImagesReelles { get; set; } = new();
}

public class ProjetUpdateForm
{
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<IFormFile> Images3D { get; set; } = new();       // nouvelles uniquement
    public List<IFormFile> ImagesReelles { get; set; } = new();
}