using ArtOfDesign.Domain.Entities.Dressings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.DTOs.Dressings.Create;

public class CreateDressingDto
{
    public string Id { get; set; } = string.Empty;
    public string Forme { get; set; } = string.Empty;
    public string TypeOuverture { get; set; } = string.Empty; //enum (fr/c)
    public string Nom { get; set; } = string.Empty;
    public decimal PrixInitial { get; set; }
    public int Reduction { get; set; }
    public string Description { get; set; } = string.Empty;

    public DimensionsDressingDto Dimensions { get; set; } = null!;
    public List<CreateDressingImageDto> Images { get; set; } = new();
    public CaracteristiquesTechniquesDressingDto CaracteristiquesTechniques
    { get; set; } = null!;
    public List<int> CouleurIds { get; set; } = new();
}
