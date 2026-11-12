using ArtOfDesign.Domain.Entities.Dressings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.DTOs.Dressings;

public class UpdateDressingDto
{
    public string Categorie { get; set; } = string.Empty;
    public string Forme { get; set; } = string.Empty;
    public string TypeOuverture { get; set; } = string.Empty; //enum (fr/c)
    public string Nom { get; set; } = string.Empty;
    public decimal PrixInitial { get; set; }
    public int Reduction { get; set; }
    public string Description { get; set; } = string.Empty;

    public DimensionsDressing Dimensions { get; set; } = null!;
    public List<ImageDressing> Images { get; set; } = new();
    public CaracteristiquesTechniquesDressing CaracteristiquesTechniques
    { get; set; } = null!;
    public List<DressingCouleur> DressingCouleurs { get; set; } = new();
}
