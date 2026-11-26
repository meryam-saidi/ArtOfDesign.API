using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.DTOs.Projets.Create;

public class CreateProjetImageDto
{
    public string Title { get; set; } = string.Empty;
    public bool Est3D { get; set; }
    public Stream FileStream { get; set; } = Stream.Null;
    public string FileName { get; set; } = string.Empty;
    public int OrdreAffichage { get; set; }
    public bool EstPrincipale { get; set; }
}
