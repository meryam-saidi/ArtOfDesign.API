using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.DTOs.Projets.Update;

public class UpdateProjetImageDto
{
    public int? Id { get; set; }
    //public string? Url { get; set; }
    public string? Title { get; set; }
    public bool? Est3D { get; set; }
    public Stream? FileStream { get; set; }
    public string? FileName { get; set; } 
    public int OrdreAffichage { get; set; }
    public bool EstPrincipale { get; set; }
}
