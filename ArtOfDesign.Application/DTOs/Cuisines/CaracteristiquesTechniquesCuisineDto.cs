using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.DTOs.Cuisines;

public class CaracteristiquesTechniquesCuisineDto
{
    public List<string> CompositionType { get; set; } = new();

    public string Caisson { get; set; } = string.Empty;

    public string Facade { get; set; } = string.Empty;
}