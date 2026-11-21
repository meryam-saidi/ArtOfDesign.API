using ArtOfDesign.Domain.Entities.Catalogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Domain.Entities.Meubles;

public class MeubleCouleur
{
    public string MeubleId { get; set; } = string.Empty;
    public int CouleurId { get; set; }

    public Meuble Meuble { get; set; } = null!;
    public Couleur Couleur { get; set; } = null!;
}
