using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.DTOs.Meubles;

public class CaracteristiquesTechniquesMeubleDto
{
    public List<string> CompositionType { get; set; } = new();

    public string Caisson { get; set; } = string.Empty;

    public string Facade { get; set; } = string.Empty;
}
