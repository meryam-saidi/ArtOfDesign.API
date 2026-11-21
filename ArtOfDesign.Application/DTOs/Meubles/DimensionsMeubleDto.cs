using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.DTOs.Meubles;

public class DimensionsMeubleDto
{
    public string Largeur { get; set; } = string.Empty;
    public string Hauteur { get; set; } = string.Empty;
    public string Profondeur { get; set; } = string.Empty;
}
