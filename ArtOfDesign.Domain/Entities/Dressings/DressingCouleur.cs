using ArtOfDesign.Domain.Entities.Catalogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Domain.Entities.Dressings;

public class DressingCouleur
{
    public string DressingId { get; set; } = string.Empty;
    public int CouleurId { get; set; }

    public Dressing Dressing { get; set; } = null!;
    public Couleur Couleur { get; set; } = null!;
}
