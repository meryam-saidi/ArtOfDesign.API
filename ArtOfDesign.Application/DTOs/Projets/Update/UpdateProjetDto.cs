using ArtOfDesign.Application.DTOs.Meubles.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.DTOs.Projets.Update;

public class UpdateProjetDto
{
    //public string Categorie { get; set; } = "Projets";
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<UpdateProjetImageDto> Images { get; set; } = new();
}
