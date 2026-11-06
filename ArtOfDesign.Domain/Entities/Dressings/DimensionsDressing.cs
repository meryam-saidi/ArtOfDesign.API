using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Domain.Entities.Dressings;

public class DimensionsDressing
{
    public int Id { get; set; }
    public string Largeur { get; set; } = string.Empty;
    public string Hauteur { get; set; } = string.Empty;
    public string Profondeur { get; set; } = string.Empty;
    
    public string DressingId { get; set; } = string.Empty;
}
