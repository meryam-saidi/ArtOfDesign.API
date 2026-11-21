using ArtOfDesign.Application.DTOs.Meubles.Create;
using ArtOfDesign.Application.DTOs.Meubles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.DTOs.Meubles.Create;

public class CreateMeubleDto
{
    public string Id { get; set; } = string.Empty;
    public string Forme { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public decimal PrixInitial { get; set; }
    public int Reduction { get; set; }
    public string Description { get; set; } = string.Empty;

    public DimensionsMeubleDto Dimensions { get; set; } = null!;
    public List<CreateMeubleImageDto> Images { get; set; } = new();
    public CaracteristiquesTechniquesMeubleDto CaracteristiquesTechniques
    { get; set; } = null!;
    public List<int> CouleurIds { get; set; } = new();
}
