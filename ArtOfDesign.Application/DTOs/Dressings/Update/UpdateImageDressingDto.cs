using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Application.DTOs.Dressings.Update;

public class UpdateImageDressingDto
{
    public int? Id { get; set; }
    public string? Url { get; set; } = string.Empty;
    public Stream? FileStream { get; set; } = Stream.Null;
    public string? FileName { get; set; } = string.Empty;
    public int OrdreAffichage { get; set; }
    public bool EstPrincipale { get; set; }
}
