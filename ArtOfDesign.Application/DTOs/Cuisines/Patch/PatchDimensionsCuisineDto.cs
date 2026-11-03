using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.DTOs.Cuisines.Patch;

public class PatchDimensionsCuisineDto
{
    public string? HauteurCaissonBas { get; set; }
    public string? ProfondeurCaissonBas { get; set; }
    public string? HauteurCaissonHaut { get; set; }
    public string? ProfondeurCaissonHaut { get; set; }
}
