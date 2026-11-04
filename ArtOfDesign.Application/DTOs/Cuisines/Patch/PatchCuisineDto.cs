using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.DTOs.Cuisines.Patch;

public class PatchCuisineDto
{
    public string? Categorie { get; set; }
    public string? Forme { get; set; }
    public string? Nom { get; set; }
    public decimal? PrixInitial { get; set; }
    public int? Reduction { get; set; }
    public string? Description { get; set; }
    public string? Nb { get; set; }

    public PatchDimensionsCuisineDto? Dimensions { get; set; }

    public PatchCaracteristiquesTechniquesCuisineDto?
        CaracteristiquesTechniques
    { get; set; }

    public List<int>? CouleurIds { get; set; }
} 