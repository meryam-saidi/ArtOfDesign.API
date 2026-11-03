using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.DTOs.Cuisines.Patch;

public class PatchCaracteristiquesTechniquesCuisineDto
{
    public List<string>? CompositionType { get; set; }
    public string? Caisson { get; set; }
    public string? Facade { get; set; }
}
