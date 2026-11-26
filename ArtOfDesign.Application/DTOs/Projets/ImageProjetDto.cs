using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.DTOs.Projets;

public class ImageProjetDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool Est3D { get; set; }
    public int OrdreAffichage { get; set; }
    public bool EstPrincipale { get; set; }
}
