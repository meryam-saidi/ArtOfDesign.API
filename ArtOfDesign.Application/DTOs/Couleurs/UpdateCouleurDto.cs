using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.DTOs.Couleurs;

public class UpdateCouleurDto
{
    public string Nom { get; set; } = string.Empty;

    public string CodeCouleur { get; set; } = string.Empty;

    /*public string ImagePresentationUrl { get; set; } = string.Empty;*/
}
